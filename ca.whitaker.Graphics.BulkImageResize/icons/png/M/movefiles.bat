@echo off
setlocal EnableDelayedExpansion

REM Create directories A to Z
for %%C in (A B C D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
    if not exist %%C mkdir %%C
)

REM Move files into respective directories
for %%F in (*) do (
    set "fileName=%%F"
    set "firstLetter=!fileName:~0,1!"
    move "%%F" "!firstLetter!" > NUL
)
