Running Tests

Unit Tests

Intergration Tests

In MemoryTests

Code Coverage
dotnet test --collect:"XPlat Code Coverage"


# Delete old  table

DROP TABLE Products

# Rename copy table

RENAME TABLE `ProductsCopy` TO `Products`

# Create a new copy table

CREATE TABLE ProductsCopy LIKE Products; INSERT ProductsCopy SELECT * FROM Products;
