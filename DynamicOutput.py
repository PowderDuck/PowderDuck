import sys;
import time;
import os;
# import threading;
# import keyboard;

# pressedString = "";
# def InputHandler() : 
#     global pressedString;
#     while True :
#         pressedKey = keyboard.read_key();
#         if(pressedKey != None and pressedKey == "z") : 
#             break;
#         pressedString += pressedKey;

# t = threading.Thread(target=InputHandler);
# t.start();

class DynamicWriter : 
    displayContent = "";

    @staticmethod
    def WriteLines(lines : list[str]) : 
        terminalDimensions = os.get_terminal_size();
        sys.stdout.write("\b" * len(DynamicWriter.displayContent));
        DynamicWriter.displayContent = "";
        for lineIndex in range(len(lines)) : 
            additionalContent = lines[lineIndex];
            offset = len(DynamicWriter.displayContent) % terminalDimensions.columns;
            DynamicWriter.displayContent += " " * (terminalDimensions.columns - offset + 1) * min(lineIndex, 1) + additionalContent;
        sys.stdout.write(DynamicWriter.displayContent);

blockCharacter = "█";
iterations = 10;

for i in range(iterations) : 
    progressBar = f"|{blockCharacter * (i + 1)}{'_' * (iterations - i - 1)}|";
    progressLine = f"Progress : \033[33m{progressBar}\033[0m";
    lines = [
                f"\r[!] Current Iteration : {i}", 
                f"[-] Previous Iteration : {i - 1}", 
                f"[+] Next Iteration : {i + 1}", 
                progressLine
    ];
    DynamicWriter.WriteLines(lines);
    time.sleep(0.5);
sys.stdout.flush();
print("\r");