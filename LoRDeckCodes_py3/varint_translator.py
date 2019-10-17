ALL_BUT_MSB = 127
JUST_MSB = 128


def pop_varint(bytes_):
    result = 0
    current_shift = 0
    bytes_popped = 0

    for i in range(len(bytes_)):
        bytes_popped += 1
        current = bytes_[i] & ALL_BUT_MSB
        result |= current << current_shift

        if (bytes_[i] & JUST_MSB) != JUST_MSB:
            bytes_ = bytes_[bytes_popped:]
            return bytes_, result

        current_shift += 7

    raise ValueError("Byte array did not contain valid varints.")


def get_varint(value):
    buff = bytearray(10)
    current_index = 0

    if value == 0:
        return bytes([0])

    while value != 0:
        byte_val = value & ALL_BUT_MSB
        value >>= 7

        if value != 0:
            byte_val |= JUST_MSB

        buff[current_index] = byte_val
        current_index += 1

    return buff[0:current_index]
