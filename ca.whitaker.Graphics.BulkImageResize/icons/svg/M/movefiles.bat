@echo off
setlocal EnableDelayedExpansion

REM Create directories A to Z and move files starting with each letter
for %%C in (A B C D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
    if not exist %%C mkdir %%C
    move "%%C*.*" "%%C" > NUL
)
