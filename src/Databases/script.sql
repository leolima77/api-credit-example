CREATE PROCEDURE dbo.GetCreditLimit
    @montlyIncome DECIMAL(10, 2),
    @creditLimit DECIMAL(10, 2) OUTPUT
AS
BEGIN
    IF @montlyIncome <= 1000
        SET @creditLimit = 1000;
    ELSE IF @montlyIncome <= 2000
        SET @creditLimit = 2000;
    ELSE
        SET @creditLimit = 5000;
END
