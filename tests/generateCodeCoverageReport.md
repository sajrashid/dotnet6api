

### run dotnet test to genereate lcov file


dotnet test --collect:"XPlat Code Coverage"

### run reportgenerator

reportgenerator -reports:"tests/TestResults/c/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

### View reports in DocsAPI/coveragereport