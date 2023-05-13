using System;
using System.IO;

class SectionStreamReader : Stream
{
    private Stream baseStream;
    private long sectionStart;
    private long sectionLength;
    private long currentPosition;

    public SectionStreamReader(Stream baseStream, long sectionStart, long sectionLength)
    {
        this.baseStream = baseStream;
        this.sectionStart = sectionStart;
        this.sectionLength = sectionLength;
        this.currentPosition = 0;
    }

    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => false;
    public override long Length => sectionLength;
    public override long Position
    {
        get => currentPosition;
        set => throw new NotSupportedException();
    }

    public override void Flush()
    {
        baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (currentPosition >= sectionLength)
            return 0;

        long bytesToRead = Math.Min(count, sectionLength - currentPosition);
        int bytesRead = baseStream.Read(buffer, offset, (int)bytesToRead);
        currentPosition += bytesRead;

        return bytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                currentPosition = Math.Max(0, Math.Min(offset, sectionLength));
                break;
            case SeekOrigin.Current:
                currentPosition = Math.Max(0, Math.Min(currentPosition + offset, sectionLength));
                break;
            case SeekOrigin.End:
                currentPosition = Math.Max(0, Math.Min(sectionLength + offset, sectionLength));
                break;
        }

        return currentPosition;
    }

    public override void SetLength(long value)
    {
        baseStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        baseStream.Write(buffer, offset, count);
    }

    // Implement other overridden methods (Write, Flush, SetLength) if necessary
}

