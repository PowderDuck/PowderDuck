import math;
import sys;

def Multiply(number : str, multiplier : str) -> str : 
    number = number[::-1];
    multiplier = multiplier[::-1];
    carrier = 0;
    multiplication = "";

    for multiplierIndex in range(len(multiplier)) : 
        for numberIndex in range(len(number)) : 
            try : 
                product = int(multiplier[multiplierIndex]) * int(number[numberIndex]) + carrier;
                carrier = math.floor(product / 10);
                shift = multiplierIndex + numberIndex;
                multiplication = Add(multiplication, 
                                     str(product % 10) if carrier <= 0 or numberIndex < len(number) - 1 else f"{product}", 
                                     shift);
            except : 
                raise Exception("[-] Integer Conversion Exception");
        carrier = 0;
    return multiplication[::-1];

def Add(source : str, addition : str, shift : int) -> str : 
        addition = addition[::-1];
        additionCarrier = 0;

        for addIndex in range(len(addition)) : 
            initiator = -1;
            while (initiator < 0 or additionCarrier > 0) : 
                initiator += 1;
                shiftIndex = addIndex + shift + initiator;
                if (len(source) <= shiftIndex) : 
                    source += "0";
                adder = int(addition[addIndex]) + additionCarrier if initiator <= 0 else additionCarrier;
                additionProduct = int(source[shiftIndex]) + adder;
                additionCarrier = math.floor(additionProduct / 10);
                source = "".join([str(additionProduct % 10) if letterIndex == shiftIndex else source[letterIndex] for letterIndex in range(len(source))]);
        return source if additionCarrier <= 0 else source + str(additionCarrier);

print(Multiply(sys.argv[1], sys.argv[2]));