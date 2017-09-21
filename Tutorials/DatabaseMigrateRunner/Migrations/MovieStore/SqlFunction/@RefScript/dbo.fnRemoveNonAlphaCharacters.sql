/****** Object:  UserDefinedFunction [dbo].[fnRemoveNonAlphaCharacters]    Script Date: 9/21/2017 7:43:17 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnRemoveNonAlphaCharacters]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fnRemoveNonAlphaCharacters]
GO

/****** Object:  UserDefinedFunction [dbo].[fnRemoveNonAlphaCharacters]    Script Date: 9/21/2017 7:43:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnRemoveNonAlphaCharacters]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE Function [dbo].[fnRemoveNonAlphaCharacters](@text NVARCHAR(MAX))
Returns NVARCHAR(MAX)
AS
Begin
    Declare @KeepValues as varchar(50)
SET @KeepValues = ''%[^a-zA-Z0-9]%''
    While PatIndex(@KeepValues, @text) > 0
SET @text = STUFF(@text, PATINDEX(@KeepValues, @text), 1, '''')
    Return @text
End
' 
END

GO


