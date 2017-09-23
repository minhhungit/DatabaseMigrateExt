/****** Object:  StoredProcedure [dbo].[GetMovieById]    Script Date: 9/22/2017 1:53:36 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMovieById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetMovieById]
GO

/****** Object:  StoredProcedure [dbo].[GetMovieById]    Script Date: 9/22/2017 1:53:36 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMovieById]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetMovieById] AS' 
END
GO

-- =============================================
-- Author:		Hung Vo
-- Create date: 2017/09/21
-- Description:	GetMovieById
-- =============================================
ALTER PROCEDURE [dbo].[GetMovieById]
	@id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
		MovieId,
		Title,
		Description,
		Storyline,
		Year,
		ReleaseDate,
		Runtime
	FROM dbo.Movie 
	WHERE MovieId = @id
END

GO


