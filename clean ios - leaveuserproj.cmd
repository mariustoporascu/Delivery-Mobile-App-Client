@echo off

call :Clean . FoodDeliveryApp

exit /b %ERRORLEVEL%

:Clean
@echo Cleaning %~1 

del /q /s %~1\%~2.iOS\bin
del /q /s %~1\%~2.iOS\obj
del /q /s %~1\LivroNotif\bin
del /q /s %~1\LivroNotif\obj

