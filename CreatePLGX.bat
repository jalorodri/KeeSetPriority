cd %cd%
cd ..

rmdir KeeSetPriority\bin /s /q
rmdir KeeSetPriority\obj /s /q
rmdir KeeSetPriority\.vs /s /q

mkdir .KeeSetPriority_temp
mkdir .KeeSetPriority_temp\.git
move KeeSetPriority\.gitattributes .KeeSetPriority_temp
move KeeSetPriority\.gitignore .KeeSetPriority_temp
move KeeSetPriority\.git\* .KeeSetPriority_temp\.git
move KeeSetPriority\LICENSE.md .KeeSetPriority_temp
move KeeSetPriority\README.md .KeeSetPriority_temp
move KeeSetPriority\version.info .KeeSetPriority_temp

"C:\Program Files\KeePass Password Safe 2\KeePass.exe" --plgx-create %cd%\KeeSetPriority

move .KeeSetPriority_temp\.gitattributes KeeSetPriority
move .KeeSetPriority_temp\.gitignore KeeSetPriority
move .KeeSetPriority_temp\.git\* KeeSetPriority\.git
move .KeeSetPriority_temp\LICENSE.md KeeSetPriority
move .KeeSetPriority_temp\README.md KeeSetPriority
move .KeeSetPriority_temp\version.info KeeSetPriority
rmdir .KeeSetPriority_temp /s /q

move KeeSetPriority.plgx KeeSetPriority

REM pause