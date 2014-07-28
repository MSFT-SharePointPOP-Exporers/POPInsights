USE [VisDataTestCOSMOS]
GO

DECLARE @RowCount INT
DECLARE @LowerUp INT
DECLARE @UpperUp INT
DECLARE @LowerDown INT
DECLARE @UpperDown INT
DECLARE @HitValUp INT
DECLARE @HitValDown INT
DECLARE @Date DATE
DECLARE @Time INT

--Up is Success
SET @LowerUp = 950
SET @UpperUp = 960

--Down is Failure
SET @LowerDown = 50
SET @UpperDown = 100
SET @RowCount = 0


/*
 * In the works. Attemping to go through all the components and create data
 * If I could figure this out, I would be able to do the same for NetIDs and FarmIDs
 *
 * DECLARE @Results table(ST nchar(10))
 * SELECT SuccessTag FROM Component
 * INTO ARRAY results
 */

WHILE @RowCount < (24*1)
BEGIN
	SET @HitValUp = ROUND( (@UpperUp - @LowerUp - 1) * RAND() + @LowerUp, 0)
	SET @HitValDown = ROUND( (@UpperDown - @LowerDown - 1) * RAND() + @LowerDown, 0)
	SET @Date = DATEADD(dd, FLOOR((@RowCount / 24)), '2014-06-13')
	SET @Time = @RowCount % 24

--Success Insert
	INSERT INTO [dbo].[ProdDollar_RandomJess]
		(Tag
		,NetworkID
		,FarmID
		,Date
		,Hour
		,NumberOfHits
		,CoorelationIDSample
		,TenantId)
	Values
		(5000
		,30
		,200
		,@Date
		,@Time
		,@HitValUp
		,NULL
		,NULL)

--Failure Insert
	INSERT INTO [dbo].[ProdDollar_RandomJess]
		(Tag
		,NetworkID
		,FarmID
		,Date
		,Hour
		,NumberOfHits
		,CoorelationIDSample
		,TenantId)
	Values
		(5001
		,30
		,200
		,@Date
		,@Time
		,@HitValDown
		,NULL
		,NULL)

	SET @RowCount = @RowCount + 1
END
