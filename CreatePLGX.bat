cd %cd%
cd ..

mkdir .KeeSetPriority_temp
mkdir .KeeSetPriority_temp\Properties
copy KeeSetPriority\*.cs .KeeSetPriority_temp
copy KeeSetPriority\Properties\* .KeeSetPriority_temp\Properties
copy KeeSetPriority\KeeSetPriority.csproj .KeeSetPriority_temp

"C:\Program Files\KeePass Password Safe 2\KeePass.exe" --plgx-create %cd%\.KeeSetPriority_temp

rmdir .KeeSetPriority_temp /s /q
move .KeeSetPriority_temp.plgx KeeSetPriority\KeeSetPriority.plgx
pause
exit