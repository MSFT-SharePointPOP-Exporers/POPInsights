BULK
INSERT DataCenter
FROM 'C:\Users\t-fitrej\Documents\DC.csv'
WITH
(
FIELDTERMINATOR = ',',
ROWTERMINATOR = '\n'
)
GO