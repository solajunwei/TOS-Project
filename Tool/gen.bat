set WORKSPACE=.
set GEN_CLIENT=%WORKSPACE%\Luban\Luban.dll
set CONF_ROOT=%WORKSPACE%\DataTables

dotnet %GEN_CLIENT% ^
	-t client ^
	-c cs-simple-json ^
	-d json ^
	--conf %CONF_ROOT%\luban.conf ^
	-x outputCodeDir=%WORKSPACE%\..\Project\Assets\Gen ^
	-x outputDataDir=%WORKSPACE%\GenerateDatas\json ^
	-x pathValidator.rootDir=%WORKSPACE%\..\Project ^
	-x l10n.provider=default^
	-x l10n.textFile.path=*@%WORKSPACE%\GenerateDatas\l10n\texts.json ^
	-x l10n.textFile.keyFieldName=key

pause