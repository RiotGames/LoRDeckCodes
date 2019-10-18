import re
from typing import IO
from base64 import b32decode


def next_varint(stream: IO) -> int:
    shift = 0
    result = 0
    while True:
        c = stream.read(1)
        if c == "":
            raise EOFError("Unexpected EOF while reading varint")
        i = ord(c)
        result |= (i & 0x7f) << shift
        shift += 7
        if not (i & 0x80):
            break

    return result


def write_varint(stream: IO, i: int) -> int:
    buf = b""
    while True:
        towrite = i & 0x7f
        i >>= 7
        if i:
            buf += bytes((towrite | 0x80,))
        else:
            buf += bytes((towrite,))
            break

    return stream.write(buf)


def decode_base32(data: str, altchars='+/') -> bytes:
    """
    Decode base32, python requires additional padding
    """
    data = re.sub(r'[^a-zA-Z0-9%s]+' % altchars, '', data)  # normalize
    missing_padding = len(data) % 8
    if missing_padding:
        data += '=' * (8 - missing_padding)
    return b32decode(data, altchars)
