﻿using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using LZ4;
using Microsoft.IO;

namespace Tavisca.Frameworks.Serialization.Compression
{
    internal sealed class CompressionProvider
    {
        private static readonly RecyclableMemoryStreamManager StreamManager = new RecyclableMemoryStreamManager();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public byte[] Compress(byte[] data, CompressionTypeOptions compressionType)
        {
            if (compressionType == CompressionTypeOptions.None)
                return data;

            if (data == null || data.Length == 0)
                return new byte[] { };
            // create an empty resultant data stream

            try
            {
                if (compressionType == CompressionTypeOptions.Lz4)
                    return CompressWithLz4(data);

                using (var outputStream = StreamManager.GetStream())
                {
                    var stream = new LeakyStream(outputStream);
                    // Create the compression stream
                    using (
                        var compressionStream = CreateCompressionStream(compressionType, stream,
                            CompressionMode.Compress))
                    {
                        compressionStream.Write(data, 0, data.Length);
                        compressionStream.Close();
                        var result = outputStream.ToArray();
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new SerializationException("An error during compression of an object. Check inner exception for further details.", ex);
            }
        }

        public byte[] DeCompress(byte[] data, CompressionTypeOptions compressionType)
        {
            try
            {
                if (compressionType == CompressionTypeOptions.None)
                    return data;

                if (data == null || data.Length == 0)
                    return new byte[] { };

                if (compressionType == CompressionTypeOptions.Lz4)
                    return DecompressWithLz4(data);

                // create an resultant data stream
                var dataStream = StreamManager.GetStream("DeCompression", data, 0, data.Length);

                // Create the compression stream
                using (var compressionStream = CreateCompressionStream(compressionType, dataStream,
                                                                       CompressionMode.Decompress))
                {
                    return ReadAllBytesFromStream(compressionStream);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("An error during decompression of an object. Check inner exception for further details.", ex);
            }
        }

        private Stream CreateCompressionStream(CompressionTypeOptions compressionType, Stream underlyingStream,
            CompressionMode mode)
        {
            switch (compressionType)
            {
                case CompressionTypeOptions.Deflate:
                    return new DeflateStream(underlyingStream, mode);
                case CompressionTypeOptions.Zip:
                default:
                    return new GZipStream(underlyingStream, mode);
            }
        }

        private byte[] ReadAllBytesFromStream(Stream stream)
        {
            // Use this method is used to read all bytes from a stream.
            const int bufferSize = 1;
            var outStream = StreamManager.GetStream();
            var buffer = new byte[bufferSize];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, bufferSize);
                if (bytesRead == 0)
                {
                    break;
                }
                outStream.Write(buffer, 0, bytesRead);
            }
            long length = outStream.Length;
            var result = new byte[length];
            outStream.Position = 0;
            outStream.Read(result, 0, (int)length);
            return result;
        }
        
        private byte[] CompressWithLz4(byte[] data)
        {
            return LZ4Codec.WrapHC(data, 0, data.Length);
        }

        private byte[] DecompressWithLz4(byte[] data)
        {
            return LZ4Codec.Unwrap(data);
        }
    }
}
