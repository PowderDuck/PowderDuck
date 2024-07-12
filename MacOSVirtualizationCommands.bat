set vBoxPath=%1
set vmName=%2

%vBoxPath% modifyvm %vmName% --cpuidset 00000001 000106e5 00100800 0098e3fd bfebfbff

%vBoxPath% setextradata %vmName% "VBoxInternal/Devices/efi/0/Config/DmiSystemProduct" "iMac19,1"

%vBoxPath% setextradata %vmName% "VBoxInternal/Devices/efi/0/Config/DmiSystemVersion" "1.0"

%vBoxPath% setextradata %vmName% "VBoxInternal/Devices/efi/0/Config/DmiBoardProduct" "Mac-AA95B1DDAB278B95"

%vBoxPath% setextradata %vmName% "VBoxInternal/Devices/smc/0/Config/DeviceKey" "ourhardworkbythesewordsguardedpleasedontsteal(c)AppleComputerInc"

