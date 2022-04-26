@echo off

call :Clean . FoodDeliveryApp

exit /b %ERRORLEVEL%

:Clean
@echo Cleaning %~1 
del /q /s %~1\%~2.Android\bin
del /q /s %~1\%~2.Android\obj


del /q /s %~1\%~2.iOS\bin
del /q /s %~1\%~2.iOS\obj


del /q /s %~1\%~2\bin
del /q /s %~1\%~2\obj
