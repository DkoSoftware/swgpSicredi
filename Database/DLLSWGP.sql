USE [SWGP]
GO
/****** Object:  Table [dbo].[UA]    Script Date: 04/08/2012 17:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UA](
	[idUA] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[idUA] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Prospect]    Script Date: 04/08/2012 17:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Prospect](
	[idProspect] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](150) NOT NULL,
	[Endereco] [nvarchar](250) NULL,
	[Telefone] [nvarchar](11) NULL,
	[Tipo] [char](2) NULL,
	[Segmento] [nvarchar](30) NULL,
	[Observacao] [nvarchar](300) NULL,
	[Email] [nvarchar](50) NULL,
	[Bairro] [nvarchar](100) NULL,
	[Cidade] [nvarchar](100) NULL,
	[Estado] [char](2) NULL,
	[DataCadastro] [datetime] NULL,
	[PessoaContato] [nvarchar](150) NULL,
	[CPF] [nvarchar](50) NULL,
	[CNPJ] [nvarchar](50) NULL,
	[FkUsuario] [int] NULL,
	[SituacaoProspect] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[idProspect] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Indicacao]    Script Date: 04/08/2012 17:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Indicacao](
	[idIndicacao] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](150) NOT NULL,
	[Telefone] [nvarchar](50) NULL,
	[Endereco] [nvarchar](150) NULL,
	[Bairro] [nvarchar](150) NULL,
	[Cidade] [nvarchar](150) NULL,
	[Estado] [nvarchar](150) NULL,
	[idUsuarioRecebe] [int] NOT NULL,
	[idUsuarioIndica] [int] NOT NULL,
 CONSTRAINT [PK_Indicacao2] PRIMARY KEY CLUSTERED 
(
	[idIndicacao] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PosicaoIndicacao]    Script Date: 04/08/2012 17:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PosicaoIndicacao](
	[idPosicaoIndicacao] [int] IDENTITY(1,1) NOT NULL,
	[NomeUsuarioRecebe] [nvarchar](50) NULL,
	[NomeUsuarioIndica] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[idPosicaoIndicacao] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 04/08/2012 17:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[idUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](200) NULL,
	[UserName] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[Cargo] [nvarchar](50) NULL,
	[Situacao] [char](1) NULL,
	[Modulo] [char](1) NULL,
	[FkUa] [int] NOT NULL,
	[Funcao] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Lembrete]    Script Date: 04/08/2012 17:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lembrete](
	[idLembrete] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [nvarchar](250) NULL,
	[DataCadastro] [datetime] NULL,
	[DataLembrar] [datetime] NULL,
	[FkUsuario] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idLembrete] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_Update]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável pela atualização de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_Update] ( 
   @Param_idIndicacao Int,
   @Param_Nome Varchar(150),
   @Param_Telefone Varchar(50),
   @Param_Endereco Varchar(150),
   @Param_Bairro Varchar(150),
   @Param_Cidade Varchar(150),
   @Param_Estado Varchar(150),
   @Param_idUsuarioRecebe Int,
   @Param_idUsuarioIndica Int) 
AS

UPDATE [dbo].[Indicacao] SET
          [Nome] = @Param_Nome, 
          [Telefone] = @Param_Telefone, 
          [Endereco] = @Param_Endereco, 
          [Bairro] = @Param_Bairro, 
          [Cidade] = @Param_Cidade, 
          [Estado] = @Param_Estado, 
          [idUsuarioRecebe] = @Param_idUsuarioRecebe, 
          [idUsuarioIndica] = @Param_idUsuarioIndica
WHERE
               [idIndicacao] = @Param_idIndicacao

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_Select]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_Select] (
   @Param_idIndicacao Int) 
AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)
      WHERE
               [idIndicacao] = @Param_idIndicacao

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_GetAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_GetAll]

AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_FindByTelefone]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_FindByTelefone] (
   @Param_Telefone Varchar(50)) 
AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)
      WHERE
               [Telefone] = @Param_Telefone


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_FindByNome]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_FindByNome] (
   @Param_Nome Varchar(150)) 
AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)
      WHERE
               [Nome] = @Param_Nome


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_FindByidUsuarioRecebe]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_FindByidUsuarioRecebe] (
   @Param_idUsuarioRecebe Int) 
AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)
      WHERE
               [idUsuarioRecebe] = @Param_idUsuarioRecebe


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_FindByidUsuarioIndica]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_FindByidUsuarioIndica] (
   @Param_idUsuarioIndica Int) 
AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)
      WHERE
               [idUsuarioIndica] = @Param_idUsuarioIndica


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_FindByEstado]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_FindByEstado] (
   @Param_Estado Varchar(150)) 
AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)
      WHERE
               [Estado] = @Param_Estado


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_FindByEndereco]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_FindByEndereco] (
   @Param_Endereco Varchar(150)) 
AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)
      WHERE
               [Endereco] = @Param_Endereco


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_FindByCidade]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_FindByCidade] (
   @Param_Cidade Varchar(150)) 
AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)
      WHERE
               [Cidade] = @Param_Cidade


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_FindByBairro]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_FindByBairro] (
   @Param_Bairro Varchar(150)) 
AS

SELECT
        [idIndicacao], 
        [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica] FROM [dbo].[Indicacao] WITH (READUNCOMMITTED)
      WHERE
               [Bairro] = @Param_Bairro


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_DeleteAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável pela Exclusão de TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_DeleteAll] 

AS

DELETE FROM [dbo].[Indicacao] 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_Delete]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável pela exclusão de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_Delete] ( 
   @Param_idIndicacao Int) 
AS

DELETE FROM [dbo].[Indicacao] 
       WHERE
               [idIndicacao] = @Param_idIndicacao

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_CountAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável pelo retorno do total de registro gravados na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_CountAll] 

AS

SELECT COUNT(*) FROM [dbo].[Indicacao] WITH (READUNCOMMITTED) 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Indicacao_Add]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Indicacao     
-- Create date: 02/04/2012 21:22:05     
-- Description: Procedimento armazenado responsável pela inserção de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Indicacao_Add] ( 
   @Param_idIndicacao Int OutPut,
   @Param_Nome Varchar(150),
   @Param_Telefone Varchar(50),
   @Param_Endereco Varchar(150),
   @Param_Bairro Varchar(150),
   @Param_Cidade Varchar(150),
   @Param_Estado Varchar(150),
   @Param_idUsuarioRecebe Int,
   @Param_idUsuarioIndica Int) 
AS

INSERT INTO [dbo].[Indicacao]
      ( [Nome], 
        [Telefone], 
        [Endereco], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [idUsuarioRecebe], 
        [idUsuarioIndica])
VALUES
      ( @Param_Nome, 
        @Param_Telefone, 
        @Param_Endereco, 
        @Param_Bairro, 
        @Param_Cidade, 
        @Param_Estado, 
        @Param_idUsuarioRecebe, 
        @Param_idUsuarioIndica)


SET @Param_idIndicacao = @@IDENTITY


RETURN
GO
/****** Object:  Table [dbo].[Contato]    Script Date: 04/08/2012 17:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contato](
	[idContato] [int] IDENTITY(1,1) NOT NULL,
	[Tipo] [nvarchar](30) NULL,
	[Descricao] [nvarchar](300) NULL,
	[Situacao] [nvarchar](30) NULL,
	[DataContato] [datetime] NULL,
	[DataCadastro] [datetime] NULL,
	[fkProspect] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idContato] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Proc_UA_Update]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: UA     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela atualização de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_UA_Update] ( 
   @Param_idUA Int,
   @Param_Nome Varchar(50)) 
AS

UPDATE [dbo].[UA] SET
          [Nome] = @Param_Nome
WHERE
               [idUA] = @Param_idUA

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_UA_Select]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: UA     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_UA_Select] (
   @Param_idUA Int) 
AS

SELECT
        [idUA], 
        [Nome] FROM [dbo].[UA] WITH (READUNCOMMITTED)
      WHERE
               [idUA] = @Param_idUA

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_UA_GetAll]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: UA     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_UA_GetAll]

AS

SELECT
        [idUA], 
        [Nome] FROM [dbo].[UA] WITH (READUNCOMMITTED)

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_UA_FindByNome]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: UA     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_UA_FindByNome] (
   @Param_Nome Varchar(50)) 
AS

SELECT
        [idUA], 
        [Nome] FROM [dbo].[UA] WITH (READUNCOMMITTED)
      WHERE
               [Nome] = @Param_Nome


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_UA_DeleteAll]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: UA     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela Exclusão de TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_UA_DeleteAll] 

AS

DELETE FROM [dbo].[UA] 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_UA_Delete]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: UA     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela exclusão de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_UA_Delete] ( 
   @Param_idUA Int) 
AS

DELETE FROM [dbo].[UA] 
       WHERE
               [idUA] = @Param_idUA

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_UA_CountAll]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: UA     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pelo retorno do total de registro gravados na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_UA_CountAll] 

AS

SELECT COUNT(*) FROM [dbo].[UA] WITH (READUNCOMMITTED) 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_UA_Add]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: UA     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela inserção de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_UA_Add] ( 
   @Param_idUA Int OutPut,
   @Param_Nome Varchar(50)) 
AS

INSERT INTO [dbo].[UA]
      ( [Nome])
VALUES
      ( @Param_Nome)


SET @Param_idUA = @@IDENTITY


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_Update]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável pela atualização de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_Update] ( 
   @Param_idProspect Int,
   @Param_Nome Varchar(150),
   @Param_Endereco Varchar(250),
   @Param_Telefone Varchar(11),
   @Param_Tipo Varchar(2),
   @Param_Segmento Varchar(30),
   @Param_Observacao Varchar(300),
   @Param_Email Varchar(50),
   @Param_Bairro Varchar(100),
   @Param_Cidade Varchar(100),
   @Param_Estado Varchar(2),
   @Param_DataCadastro SmallDateTime,
   @Param_PessoaContato Varchar(150),
   @Param_CPF Varchar(50),
   @Param_CNPJ Varchar(50),
   @Param_FkUsuario Int,
   @Param_SituacaoProspect Varchar(20)) 
AS

UPDATE [dbo].[Prospect] SET
          [Nome] = @Param_Nome, 
          [Endereco] = @Param_Endereco, 
          [Telefone] = @Param_Telefone, 
          [Tipo] = @Param_Tipo, 
          [Segmento] = @Param_Segmento, 
          [Observacao] = @Param_Observacao, 
          [Email] = @Param_Email, 
          [Bairro] = @Param_Bairro, 
          [Cidade] = @Param_Cidade, 
          [Estado] = @Param_Estado, 
          [DataCadastro] = @Param_DataCadastro, 
          [PessoaContato] = @Param_PessoaContato, 
          [CPF] = @Param_CPF, 
          [CNPJ] = @Param_CNPJ, 
          [FkUsuario] = @Param_FkUsuario, 
          [SituacaoProspect] = @Param_SituacaoProspect
WHERE
               [idProspect] = @Param_idProspect

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_Select]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_Select] (
   @Param_idProspect Int) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [idProspect] = @Param_idProspect

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_GetAll]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_GetAll]

AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByTipo]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByTipo] (
   @Param_Tipo Varchar(2)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Tipo] = @Param_Tipo


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByTelefone]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByTelefone] (
   @Param_Telefone Varchar(11)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Telefone] = @Param_Telefone


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindBySituacaoProspect]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindBySituacaoProspect] (
   @Param_SituacaoProspect Varchar(20)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [SituacaoProspect] = @Param_SituacaoProspect


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindBySegmento]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindBySegmento] (
   @Param_Segmento Varchar(30)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Segmento] = @Param_Segmento


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByPessoaContato]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByPessoaContato] (
   @Param_PessoaContato Varchar(150)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [PessoaContato] = @Param_PessoaContato


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByObservacao]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByObservacao] (
   @Param_Observacao Varchar(300)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Observacao] = @Param_Observacao


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByNome]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByNome] (
   @Param_Nome Varchar(150)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Nome] = @Param_Nome


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByFkUsuario]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByFkUsuario] (
   @Param_FkUsuario Int) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [FkUsuario] = @Param_FkUsuario


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByEstado]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByEstado] (
   @Param_Estado Varchar(2)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Estado] = @Param_Estado


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByEndereco]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByEndereco] (
   @Param_Endereco Varchar(250)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Endereco] = @Param_Endereco


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByEmail]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByEmail] (
   @Param_Email Varchar(50)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Email] = @Param_Email


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByDataCadastro]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByDataCadastro] (
   @Param_DataCadastro SmallDateTime) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [DataCadastro] = @Param_DataCadastro


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByCPF]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByCPF] (
   @Param_CPF Varchar(50)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [CPF] = @Param_CPF


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByCNPJ]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByCNPJ] (
   @Param_CNPJ Varchar(50)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [CNPJ] = @Param_CNPJ


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByCidade]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByCidade] (
   @Param_Cidade Varchar(100)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Cidade] = @Param_Cidade


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_FindByBairro]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_FindByBairro] (
   @Param_Bairro Varchar(100)) 
AS

SELECT
        [idProspect], 
        [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect] FROM [dbo].[Prospect] WITH (READUNCOMMITTED)
      WHERE
               [Bairro] = @Param_Bairro


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_DeleteAll]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável pela Exclusão de TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_DeleteAll] 

AS

DELETE FROM [dbo].[Prospect] 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_Delete]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável pela exclusão de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_Delete] ( 
   @Param_idProspect Int) 
AS

DELETE FROM [dbo].[Prospect] 
       WHERE
               [idProspect] = @Param_idProspect

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_CountAll]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável pelo retorno do total de registro gravados na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_CountAll] 

AS

SELECT COUNT(*) FROM [dbo].[Prospect] WITH (READUNCOMMITTED) 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Prospect_Add]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Prospect     
-- Create date: 26/03/2012 23:14:20     
-- Description: Procedimento armazenado responsável pela inserção de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Prospect_Add] ( 
   @Param_idProspect Int OutPut,
   @Param_Nome Varchar(150),
   @Param_Endereco Varchar(250),
   @Param_Telefone Varchar(11),
   @Param_Tipo Varchar(2),
   @Param_Segmento Varchar(30),
   @Param_Observacao Varchar(300),
   @Param_Email Varchar(50),
   @Param_Bairro Varchar(100),
   @Param_Cidade Varchar(100),
   @Param_Estado Varchar(2),
   @Param_DataCadastro SmallDateTime,
   @Param_PessoaContato Varchar(150),
   @Param_CPF Varchar(50),
   @Param_CNPJ Varchar(50),
   @Param_FkUsuario Int,
   @Param_SituacaoProspect Varchar(20)) 
AS

INSERT INTO [dbo].[Prospect]
      ( [Nome], 
        [Endereco], 
        [Telefone], 
        [Tipo], 
        [Segmento], 
        [Observacao], 
        [Email], 
        [Bairro], 
        [Cidade], 
        [Estado], 
        [DataCadastro], 
        [PessoaContato], 
        [CPF], 
        [CNPJ], 
        [FkUsuario], 
        [SituacaoProspect])
VALUES
      ( @Param_Nome, 
        @Param_Endereco, 
        @Param_Telefone, 
        @Param_Tipo, 
        @Param_Segmento, 
        @Param_Observacao, 
        @Param_Email, 
        @Param_Bairro, 
        @Param_Cidade, 
        @Param_Estado, 
        @Param_DataCadastro, 
        @Param_PessoaContato, 
        @Param_CPF, 
        @Param_CNPJ, 
        @Param_FkUsuario, 
        @Param_SituacaoProspect)


SET @Param_idProspect = @@IDENTITY


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_PosicaoIndicacao_Update]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: PosicaoIndicacao     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela atualização de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_PosicaoIndicacao_Update] ( 
   @Param_idPosicaoIndicacao Int,
   @Param_NomeUsuarioRecebe Varchar(50),
   @Param_NomeUsuarioIndica Varchar(50)) 
AS

UPDATE [dbo].[PosicaoIndicacao] SET
          [NomeUsuarioRecebe] = @Param_NomeUsuarioRecebe, 
          [NomeUsuarioIndica] = @Param_NomeUsuarioIndica
WHERE
               [idPosicaoIndicacao] = @Param_idPosicaoIndicacao

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_PosicaoIndicacao_Select]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: PosicaoIndicacao     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_PosicaoIndicacao_Select] (
   @Param_idPosicaoIndicacao Int) 
AS

SELECT
        [idPosicaoIndicacao], 
        [NomeUsuarioRecebe], 
        [NomeUsuarioIndica] FROM [dbo].[PosicaoIndicacao] WITH (READUNCOMMITTED)
      WHERE
               [idPosicaoIndicacao] = @Param_idPosicaoIndicacao

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_PosicaoIndicacao_GetAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: PosicaoIndicacao     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_PosicaoIndicacao_GetAll]

AS

SELECT
        [idPosicaoIndicacao], 
        [NomeUsuarioRecebe], 
        [NomeUsuarioIndica] FROM [dbo].[PosicaoIndicacao] WITH (READUNCOMMITTED)

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_PosicaoIndicacao_FindByNomeUsuarioRecebe]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: PosicaoIndicacao     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_PosicaoIndicacao_FindByNomeUsuarioRecebe] (
   @Param_NomeUsuarioRecebe Varchar(50)) 
AS

SELECT
        [idPosicaoIndicacao], 
        [NomeUsuarioRecebe], 
        [NomeUsuarioIndica] FROM [dbo].[PosicaoIndicacao] WITH (READUNCOMMITTED)
      WHERE
               [NomeUsuarioRecebe] = @Param_NomeUsuarioRecebe


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_PosicaoIndicacao_FindByNomeUsuarioIndica]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: PosicaoIndicacao     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_PosicaoIndicacao_FindByNomeUsuarioIndica] (
   @Param_NomeUsuarioIndica Varchar(50)) 
AS

SELECT
        [idPosicaoIndicacao], 
        [NomeUsuarioRecebe], 
        [NomeUsuarioIndica] FROM [dbo].[PosicaoIndicacao] WITH (READUNCOMMITTED)
      WHERE
               [NomeUsuarioIndica] = @Param_NomeUsuarioIndica


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_PosicaoIndicacao_DeleteAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: PosicaoIndicacao     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela Exclusão de TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_PosicaoIndicacao_DeleteAll] 

AS

DELETE FROM [dbo].[PosicaoIndicacao] 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_PosicaoIndicacao_Delete]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: PosicaoIndicacao     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela exclusão de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_PosicaoIndicacao_Delete] ( 
   @Param_idPosicaoIndicacao Int) 
AS

DELETE FROM [dbo].[PosicaoIndicacao] 
       WHERE
               [idPosicaoIndicacao] = @Param_idPosicaoIndicacao

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_PosicaoIndicacao_CountAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: PosicaoIndicacao     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pelo retorno do total de registro gravados na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_PosicaoIndicacao_CountAll] 

AS

SELECT COUNT(*) FROM [dbo].[PosicaoIndicacao] WITH (READUNCOMMITTED) 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_PosicaoIndicacao_Add]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: PosicaoIndicacao     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela inserção de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_PosicaoIndicacao_Add] ( 
   @Param_idPosicaoIndicacao Int OutPut,
   @Param_NomeUsuarioRecebe Varchar(50),
   @Param_NomeUsuarioIndica Varchar(50)) 
AS

INSERT INTO [dbo].[PosicaoIndicacao]
      ( [NomeUsuarioRecebe], 
        [NomeUsuarioIndica])
VALUES
      ( @Param_NomeUsuarioRecebe, 
        @Param_NomeUsuarioIndica)


SET @Param_idPosicaoIndicacao = @@IDENTITY


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_Update]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável pela atualização de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_Update] ( 
   @Param_idUsuario Int,
   @Param_Nome Varchar(200),
   @Param_UserName Varchar(100),
   @Param_Password Varchar(100),
   @Param_Cargo Varchar(50),
   @Param_Situacao Varchar(1),
   @Param_Modulo Varchar(1),
   @Param_FkUa Int,
   @Param_Funcao Varchar(30)) 
AS

UPDATE [dbo].[Usuario] SET
          [Nome] = @Param_Nome, 
          [UserName] = @Param_UserName, 
          [Password] = @Param_Password, 
          [Cargo] = @Param_Cargo, 
          [Situacao] = @Param_Situacao, 
          [Modulo] = @Param_Modulo, 
          [FkUa] = @Param_FkUa, 
          [Funcao] = @Param_Funcao
WHERE
               [idUsuario] = @Param_idUsuario

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_Select]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_Select] (
   @Param_idUsuario Int) 
AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)
      WHERE
               [idUsuario] = @Param_idUsuario

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_GetAll]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_GetAll]

AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_FindByUserName]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_FindByUserName] (
   @Param_UserName Varchar(100)) 
AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)
      WHERE
               [UserName] = @Param_UserName


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_FindBySituacao]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_FindBySituacao] (
   @Param_Situacao Varchar(1)) 
AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)
      WHERE
               [Situacao] = @Param_Situacao


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_FindByPassword]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_FindByPassword] (
   @Param_Password Varchar(100)) 
AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)
      WHERE
               [Password] = @Param_Password


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_FindByNome]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_FindByNome] (
   @Param_Nome Varchar(200)) 
AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)
      WHERE
               [Nome] = @Param_Nome


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_FindByModulo]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_FindByModulo] (
   @Param_Modulo Varchar(1)) 
AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)
      WHERE
               [Modulo] = @Param_Modulo


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_FindByFuncao]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_FindByFuncao] (
   @Param_Funcao Varchar(30)) 
AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)
      WHERE
               [Funcao] = @Param_Funcao


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_FindByFkUa]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_FindByFkUa] (
   @Param_FkUa Int) 
AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)
      WHERE
               [FkUa] = @Param_FkUa


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_FindByCargo]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_FindByCargo] (
   @Param_Cargo Varchar(50)) 
AS

SELECT
        [idUsuario], 
        [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao] FROM [dbo].[Usuario] WITH (READUNCOMMITTED)
      WHERE
               [Cargo] = @Param_Cargo


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_DeleteAll]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável pela Exclusão de TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_DeleteAll] 

AS

DELETE FROM [dbo].[Usuario] 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_Delete]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável pela exclusão de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_Delete] ( 
   @Param_idUsuario Int) 
AS

DELETE FROM [dbo].[Usuario] 
       WHERE
               [idUsuario] = @Param_idUsuario

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_CountAll]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável pelo retorno do total de registro gravados na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_CountAll] 

AS

SELECT COUNT(*) FROM [dbo].[Usuario] WITH (READUNCOMMITTED) 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Usuario_Add]    Script Date: 04/08/2012 17:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Usuario     
-- Create date: 24/03/2012 20:08:17     
-- Description: Procedimento armazenado responsável pela inserção de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Usuario_Add] ( 
   @Param_idUsuario Int OutPut,
   @Param_Nome Varchar(200),
   @Param_UserName Varchar(100),
   @Param_Password Varchar(100),
   @Param_Cargo Varchar(50),
   @Param_Situacao Varchar(1),
   @Param_Modulo Varchar(1),
   @Param_FkUa Int,
   @Param_Funcao Varchar(30)) 
AS

INSERT INTO [dbo].[Usuario]
      ( [Nome], 
        [UserName], 
        [Password], 
        [Cargo], 
        [Situacao], 
        [Modulo], 
        [FkUa], 
        [Funcao])
VALUES
      ( @Param_Nome, 
        @Param_UserName, 
        @Param_Password, 
        @Param_Cargo, 
        @Param_Situacao, 
        @Param_Modulo, 
        @Param_FkUa, 
        @Param_Funcao)


SET @Param_idUsuario = @@IDENTITY


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_Update]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela atualização de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_Update] ( 
   @Param_idLembrete Int,
   @Param_Descricao Varchar(250),
   @Param_DataCadastro SmallDateTime,
   @Param_DataLembrar SmallDateTime,
   @Param_FkUsuario Int) 
AS

UPDATE [dbo].[Lembrete] SET
          [Descricao] = @Param_Descricao, 
          [DataCadastro] = @Param_DataCadastro, 
          [DataLembrar] = @Param_DataLembrar, 
          [FkUsuario] = @Param_FkUsuario
WHERE
               [idLembrete] = @Param_idLembrete

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_Select]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_Select] (
   @Param_idLembrete Int) 
AS

SELECT
        [idLembrete], 
        [Descricao], 
        [DataCadastro], 
        [DataLembrar], 
        [FkUsuario] FROM [dbo].[Lembrete] WITH (READUNCOMMITTED)
      WHERE
               [idLembrete] = @Param_idLembrete

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_GetAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_GetAll]

AS

SELECT
        [idLembrete], 
        [Descricao], 
        [DataCadastro], 
        [DataLembrar], 
        [FkUsuario] FROM [dbo].[Lembrete] WITH (READUNCOMMITTED)

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_FindByFkUsuario]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_FindByFkUsuario] (
   @Param_FkUsuario Int) 
AS

SELECT
        [idLembrete], 
        [Descricao], 
        [DataCadastro], 
        [DataLembrar], 
        [FkUsuario] FROM [dbo].[Lembrete] WITH (READUNCOMMITTED)
      WHERE
               [FkUsuario] = @Param_FkUsuario


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_FindByDescricao]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_FindByDescricao] (
   @Param_Descricao Varchar(250)) 
AS

SELECT
        [idLembrete], 
        [Descricao], 
        [DataCadastro], 
        [DataLembrar], 
        [FkUsuario] FROM [dbo].[Lembrete] WITH (READUNCOMMITTED)
      WHERE
               [Descricao] = @Param_Descricao


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_FindByDataLembrar]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_FindByDataLembrar] (
   @Param_DataLembrar SmallDateTime) 
AS

SELECT
        [idLembrete], 
        [Descricao], 
        [DataCadastro], 
        [DataLembrar], 
        [FkUsuario] FROM [dbo].[Lembrete] WITH (READUNCOMMITTED)
      WHERE
               [DataLembrar] = @Param_DataLembrar


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_FindByDataCadastro]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_FindByDataCadastro] (
   @Param_DataCadastro SmallDateTime) 
AS

SELECT
        [idLembrete], 
        [Descricao], 
        [DataCadastro], 
        [DataLembrar], 
        [FkUsuario] FROM [dbo].[Lembrete] WITH (READUNCOMMITTED)
      WHERE
               [DataCadastro] = @Param_DataCadastro


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_DeleteAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela Exclusão de TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_DeleteAll] 

AS

DELETE FROM [dbo].[Lembrete] 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_Delete]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela exclusão de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_Delete] ( 
   @Param_idLembrete Int) 
AS

DELETE FROM [dbo].[Lembrete] 
       WHERE
               [idLembrete] = @Param_idLembrete

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_CountAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pelo retorno do total de registro gravados na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_CountAll] 

AS

SELECT COUNT(*) FROM [dbo].[Lembrete] WITH (READUNCOMMITTED) 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Lembrete_Add]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Lembrete     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela inserção de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Lembrete_Add] ( 
   @Param_idLembrete Int OutPut,
   @Param_Descricao Varchar(250),
   @Param_DataCadastro SmallDateTime,
   @Param_DataLembrar SmallDateTime,
   @Param_FkUsuario Int) 
AS

INSERT INTO [dbo].[Lembrete]
      ( [Descricao], 
        [DataCadastro], 
        [DataLembrar], 
        [FkUsuario])
VALUES
      ( @Param_Descricao, 
        @Param_DataCadastro, 
        @Param_DataLembrar, 
        @Param_FkUsuario)


SET @Param_idLembrete = @@IDENTITY


RETURN
GO
/****** Object:  Table [dbo].[Associacao]    Script Date: 04/08/2012 17:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Associacao](
	[idAssociacao] [int] IDENTITY(1,1) NOT NULL,
	[fkContato] [int] NOT NULL,
	[NumeroConta] [nvarchar](7) NOT NULL,
	[DataAssociacao] [datetime] NULL,
	[DataCadastro] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[idAssociacao] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_Update]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela atualização de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_Update] ( 
   @Param_idContato Int,
   @Param_Tipo Varchar(30),
   @Param_Descricao Varchar(300),
   @Param_Situacao Varchar(30),
   @Param_DataContato SmallDateTime,
   @Param_DataCadastro SmallDateTime,
   @Param_fkProspect Int) 
AS

UPDATE [dbo].[Contato] SET
          [Tipo] = @Param_Tipo, 
          [Descricao] = @Param_Descricao, 
          [Situacao] = @Param_Situacao, 
          [DataContato] = @Param_DataContato, 
          [DataCadastro] = @Param_DataCadastro, 
          [fkProspect] = @Param_fkProspect
WHERE
               [idContato] = @Param_idContato

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_Select]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_Select] (
   @Param_idContato Int) 
AS

SELECT
        [idContato], 
        [Tipo], 
        [Descricao], 
        [Situacao], 
        [DataContato], 
        [DataCadastro], 
        [fkProspect] FROM [dbo].[Contato] WITH (READUNCOMMITTED)
      WHERE
               [idContato] = @Param_idContato

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_GetAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_GetAll]

AS

SELECT
        [idContato], 
        [Tipo], 
        [Descricao], 
        [Situacao], 
        [DataContato], 
        [DataCadastro], 
        [fkProspect] FROM [dbo].[Contato] WITH (READUNCOMMITTED)

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_FindByTipo]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_FindByTipo] (
   @Param_Tipo Varchar(30)) 
AS

SELECT
        [idContato], 
        [Tipo], 
        [Descricao], 
        [Situacao], 
        [DataContato], 
        [DataCadastro], 
        [fkProspect] FROM [dbo].[Contato] WITH (READUNCOMMITTED)
      WHERE
               [Tipo] = @Param_Tipo


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_FindBySituacao]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_FindBySituacao] (
   @Param_Situacao Varchar(30)) 
AS

SELECT
        [idContato], 
        [Tipo], 
        [Descricao], 
        [Situacao], 
        [DataContato], 
        [DataCadastro], 
        [fkProspect] FROM [dbo].[Contato] WITH (READUNCOMMITTED)
      WHERE
               [Situacao] = @Param_Situacao


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_FindByfkProspect]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_FindByfkProspect] (
   @Param_fkProspect Int) 
AS

SELECT
        [idContato], 
        [Tipo], 
        [Descricao], 
        [Situacao], 
        [DataContato], 
        [DataCadastro], 
        [fkProspect] FROM [dbo].[Contato] WITH (READUNCOMMITTED)
      WHERE
               [fkProspect] = @Param_fkProspect


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_FindByDescricao]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_FindByDescricao] (
   @Param_Descricao Varchar(300)) 
AS

SELECT
        [idContato], 
        [Tipo], 
        [Descricao], 
        [Situacao], 
        [DataContato], 
        [DataCadastro], 
        [fkProspect] FROM [dbo].[Contato] WITH (READUNCOMMITTED)
      WHERE
               [Descricao] = @Param_Descricao


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_FindByDataContato]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_FindByDataContato] (
   @Param_DataContato SmallDateTime) 
AS

SELECT
        [idContato], 
        [Tipo], 
        [Descricao], 
        [Situacao], 
        [DataContato], 
        [DataCadastro], 
        [fkProspect] FROM [dbo].[Contato] WITH (READUNCOMMITTED)
      WHERE
               [DataContato] = @Param_DataContato


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_FindByDataCadastro]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_FindByDataCadastro] (
   @Param_DataCadastro SmallDateTime) 
AS

SELECT
        [idContato], 
        [Tipo], 
        [Descricao], 
        [Situacao], 
        [DataContato], 
        [DataCadastro], 
        [fkProspect] FROM [dbo].[Contato] WITH (READUNCOMMITTED)
      WHERE
               [DataCadastro] = @Param_DataCadastro


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_DeleteAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela Exclusão de TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_DeleteAll] 

AS

DELETE FROM [dbo].[Contato] 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_Delete]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela exclusão de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_Delete] ( 
   @Param_idContato Int) 
AS

DELETE FROM [dbo].[Contato] 
       WHERE
               [idContato] = @Param_idContato

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_CountAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pelo retorno do total de registro gravados na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_CountAll] 

AS

SELECT COUNT(*) FROM [dbo].[Contato] WITH (READUNCOMMITTED) 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Contato_Add]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Contato     
-- Create date: 19/03/2012 23:01:45     
-- Description: Procedimento armazenado responsável pela inserção de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Contato_Add] ( 
   @Param_idContato Int OutPut,
   @Param_Tipo Varchar(30),
   @Param_Descricao Varchar(300),
   @Param_Situacao Varchar(30),
   @Param_DataContato SmallDateTime,
   @Param_DataCadastro SmallDateTime,
   @Param_fkProspect Int) 
AS

INSERT INTO [dbo].[Contato]
      ( [Tipo], 
        [Descricao], 
        [Situacao], 
        [DataContato], 
        [DataCadastro], 
        [fkProspect])
VALUES
      ( @Param_Tipo, 
        @Param_Descricao, 
        @Param_Situacao, 
        @Param_DataContato, 
        @Param_DataCadastro, 
        @Param_fkProspect)


SET @Param_idContato = @@IDENTITY


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_Update]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável pela atualização de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_Update] ( 
   @Param_idAssociacao Int,
   @Param_fkContato Int,
   @Param_NumeroConta Varchar(7),
   @Param_DataAssociacao SmallDateTime,
   @Param_DataCadastro SmallDateTime) 
AS

UPDATE [dbo].[Associacao] SET
          [fkContato] = @Param_fkContato, 
          [NumeroConta] = @Param_NumeroConta, 
          [DataAssociacao] = @Param_DataAssociacao, 
          [DataCadastro] = @Param_DataCadastro
WHERE
               [idAssociacao] = @Param_idAssociacao

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_Select]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_Select] (
   @Param_idAssociacao Int) 
AS

SELECT
        [idAssociacao], 
        [fkContato], 
        [NumeroConta], 
        [DataAssociacao], 
        [DataCadastro] FROM [dbo].[Associacao] WITH (READUNCOMMITTED)
      WHERE
               [idAssociacao] = @Param_idAssociacao

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_GetAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável por selecionar TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_GetAll]

AS

SELECT
        [idAssociacao], 
        [fkContato], 
        [NumeroConta], 
        [DataAssociacao], 
        [DataCadastro] FROM [dbo].[Associacao] WITH (READUNCOMMITTED)

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_FindByNumeroConta]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_FindByNumeroConta] (
   @Param_NumeroConta Varchar(7)) 
AS

SELECT
        [idAssociacao], 
        [fkContato], 
        [NumeroConta], 
        [DataAssociacao], 
        [DataCadastro] FROM [dbo].[Associacao] WITH (READUNCOMMITTED)
      WHERE
               [NumeroConta] = @Param_NumeroConta


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_FindByfkContato]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_FindByfkContato] (
   @Param_fkContato Int) 
AS

SELECT
        [idAssociacao], 
        [fkContato], 
        [NumeroConta], 
        [DataAssociacao], 
        [DataCadastro] FROM [dbo].[Associacao] WITH (READUNCOMMITTED)
      WHERE
               [fkContato] = @Param_fkContato


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_FindByDataCadastro]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_FindByDataCadastro] (
   @Param_DataCadastro SmallDateTime) 
AS

SELECT
        [idAssociacao], 
        [fkContato], 
        [NumeroConta], 
        [DataAssociacao], 
        [DataCadastro] FROM [dbo].[Associacao] WITH (READUNCOMMITTED)
      WHERE
               [DataCadastro] = @Param_DataCadastro


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_FindByDataAssociacao]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável por selecionar registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_FindByDataAssociacao] (
   @Param_DataAssociacao SmallDateTime) 
AS

SELECT
        [idAssociacao], 
        [fkContato], 
        [NumeroConta], 
        [DataAssociacao], 
        [DataCadastro] FROM [dbo].[Associacao] WITH (READUNCOMMITTED)
      WHERE
               [DataAssociacao] = @Param_DataAssociacao


RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_DeleteAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável pela Exclusão de TODOS os registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_DeleteAll] 

AS

DELETE FROM [dbo].[Associacao] 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_Delete]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável pela exclusão de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_Delete] ( 
   @Param_idAssociacao Int) 
AS

DELETE FROM [dbo].[Associacao] 
       WHERE
               [idAssociacao] = @Param_idAssociacao

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_CountAll]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável pelo retorno do total de registro gravados na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_CountAll] 

AS

SELECT COUNT(*) FROM [dbo].[Associacao] WITH (READUNCOMMITTED) 

RETURN
GO
/****** Object:  StoredProcedure [dbo].[Proc_Associacao_Add]    Script Date: 04/08/2012 17:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author: DAL Creator .net 
-- Table: Associacao     
-- Create date: 25/03/2012 03:26:44     
-- Description: Procedimento armazenado responsável pela inserção de registros na tabela. 
-- ============================================= 
Create Proc [dbo].[Proc_Associacao_Add] ( 
   @Param_idAssociacao Int OutPut,
   @Param_fkContato Int,
   @Param_NumeroConta Varchar(7),
   @Param_DataAssociacao SmallDateTime,
   @Param_DataCadastro SmallDateTime) 
AS

INSERT INTO [dbo].[Associacao]
      ( [fkContato], 
        [NumeroConta], 
        [DataAssociacao], 
        [DataCadastro])
VALUES
      ( @Param_fkContato, 
        @Param_NumeroConta, 
        @Param_DataAssociacao, 
        @Param_DataCadastro)


SET @Param_idAssociacao = @@IDENTITY


RETURN
GO
/****** Object:  ForeignKey [FK_Contato]    Script Date: 04/08/2012 17:20:55 ******/
ALTER TABLE [dbo].[Associacao]  WITH CHECK ADD  CONSTRAINT [FK_Contato] FOREIGN KEY([fkContato])
REFERENCES [dbo].[Contato] ([idContato])
GO
ALTER TABLE [dbo].[Associacao] CHECK CONSTRAINT [FK_Contato]
GO
/****** Object:  ForeignKey [ProspectFk]    Script Date: 04/08/2012 17:20:55 ******/
ALTER TABLE [dbo].[Contato]  WITH CHECK ADD  CONSTRAINT [ProspectFk] FOREIGN KEY([fkProspect])
REFERENCES [dbo].[Prospect] ([idProspect])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Contato] CHECK CONSTRAINT [ProspectFk]
GO
/****** Object:  ForeignKey [UsuarioFK]    Script Date: 04/08/2012 17:20:55 ******/
ALTER TABLE [dbo].[Lembrete]  WITH CHECK ADD  CONSTRAINT [UsuarioFK] FOREIGN KEY([FkUsuario])
REFERENCES [dbo].[Prospect] ([idProspect])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Lembrete] CHECK CONSTRAINT [UsuarioFK]
GO
/****** Object:  ForeignKey [UaFk]    Script Date: 04/08/2012 17:20:55 ******/
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [UaFk] FOREIGN KEY([FkUa])
REFERENCES [dbo].[UA] ([idUA])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [UaFk]
GO

INSERT INTO [SWGP].[dbo].[UA]
           ([Nome])
     VALUES
           ('SUREG')
GO


INSERT INTO [SWGP].[dbo].[Usuario]
           ([Nome]
           ,[UserName]
           ,[Password]
           ,[Cargo]
           ,[Situacao]
           ,[Modulo]
           ,[FkUa]
           ,[Funcao])
     
     VALUES(
            'admin'
           ,'admin'
           ,'E10ADC3949BA59ABBE56E057F20F883E'
           ,'GUA'
           ,'A'
           ,'M'
           ,'1'
           ,'GUA')
GO
