using Super.Paula.Application.Administration.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public class InspectorAvatarStream : Stream
{
    private static readonly IList<byte[]> validTypes = new List<byte[]>
{
    /* jpg */ new byte[] { 0xFF, 0xD8 },
    /* bmp */ new byte[] { 0x42, 0x4 },
    /* gif */ new byte[] { 0x47, 0x49, 0x46 },
    /* png */ new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
};

    private readonly Stream _innerStream;

    private readonly byte[] _headerBytes = new byte[8];

    private int _headerBytesWritten = 0;

    public InspectorAvatarStream(Stream innerStream)
    {
        _innerStream = innerStream;
    }

    public override bool CanRead
        => _innerStream.CanRead;

    public override bool CanSeek
        => _innerStream.CanSeek;

    public override bool CanWrite
        => _innerStream.CanWrite;

    public override long Length
        => _innerStream.Length;

    public override long Position
    {
        get => _innerStream.Position;
        set => _innerStream.Position = value;
    }

    public override void Flush()
        => _innerStream.Flush();

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        var readBytes = await _innerStream.ReadAsync(buffer, offset, count, cancellationToken);

        if (offset < _headerBytes.Length)
        {
            CopyHeader(buffer, count);
            ValidateHeader();
        }

        return readBytes;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var readBytes = _innerStream.Read(buffer, offset, count);

        if (offset < _headerBytes.Length)
        {
            CopyHeader(buffer, count);
            ValidateHeader();
        }

        return readBytes;
    }

    private void CopyHeader(byte[] buffer, int count)
    {
        var bytesToCopy = Math.Min(count, _headerBytes.Length - _headerBytesWritten);
        Array.Copy(buffer, 0, _headerBytes, _headerBytesWritten, bytesToCopy);
        _headerBytesWritten += bytesToCopy;
    }

    public void ValidateHeader()
    {
        if (_headerBytesWritten != _headerBytes.Length)
        {
            return;
        }

        var imageIsValid = validTypes
            .Any(x =>
                x.SequenceEqual(
                    _headerBytes
                        .AsSpan()
                        .Slice(0, x.Length)
                        .ToArray()));

        if (!imageIsValid)
        {
            throw new InspectorAvatarStreamInvalidException($"Unsupported image type for inspector avatar.");
        }
    }

    public override long Seek(long offset, SeekOrigin origin)
        => _innerStream.Seek(offset, origin);

    public override void SetLength(long value)
        => _innerStream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count)
        => _innerStream.Write(buffer, offset, count);
}