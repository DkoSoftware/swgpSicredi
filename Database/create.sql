--CREATE DATABASE SWGP

/*
	CONSTRAINT SegmentoClienteFk FOREIGN KEY (fkSegmentoCliente)
	REFERENCES SegmentoCliente on Update CASCADE ON DELETE CASCADE
*/

CREATE TABLE UA(

	idUA int IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Nome nvarchar(50)
    
);

CREATE TABLE Usuario(

	idUsuario int IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Nome nvarchar(200),
    UserName nvarchar(100),
    Password nvarchar(100),
    Cargo nvarchar(50),
    Situacao char(1), -- (A)ativo ou (I)inativo
    Modulo char(1), --(A)administrador , (U) usuario
    FkUa int NOT NULL,
    
    CONSTRAINT UaFk FOREIGN KEY (FkUa)
	REFERENCES Ua(idUA) on Update CASCADE ON DELETE CASCADE
    
);

CREATE TABLE Prospect(
	
	idProspect int IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Nome nvarchar(150) NOT NULL,
    Endereco nvarchar(250),
    Telefone nvarchar(11),
    Tipo char(2), -- PJ ou PF
    Segmento nvarchar(30),
    Observacao nvarchar(300),
    Email nvarchar(50),
    Bairro nvarchar(100),
    Cidade nvarchar(100),
    Estado char(2),
    DataCadastro datetime
    
);

CREATE TABLE Contato(
	
	idContato int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Tipo nvarchar(30),
    Descricao nvarchar(300),
    Situacao nvarchar(30),
    DataContato datetime,
    DataCadastro datetime,
    fkProspect int NOT NULL,
    
    CONSTRAINT ProspectFk FOREIGN KEY (fkProspect)
	REFERENCES Prospect(idProspect) on Update CASCADE ON DELETE CASCADE
    
);

CREATE TABLE Associacao(

	idAssociacao int IDENTITY(1,1) PRIMARY KEY NOT NULL,
    fkProspect int NOT NULL,
    NumeroConta nvarchar(7) NOT NULL,
    DataAssociacao datetime,
    DataCadastro datetime,
    
    FOREIGN KEY( fkProspect)
	REFERENCES Prospect(idProspect) on Update CASCADE ON DELETE CASCADE
); 

CREATE TABLE PosicaoIndicacao(
	
	idPosicaoIndicacao int IDENTITY(1,1) PRIMARY KEY NOT NULL,
    NomeUsuarioRecebe nvarchar(50),
    NomeUsuarioIndica nvarchar(50)
        
);

CREATE TABLE Indicacao(

	idIndicacao int IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Nome nvarchar(150) NOT NULL,
    Endereco nvarchar(200),
    Bairro nvarchar(100),
    Cidade nvarchar(150),
    Estado char(2),
    Telefone nvarchar(11),
    FkPosicaoIndicacao int NOT NULL,
    
    CONSTRAINT PosicaoIndicacaoFK FOREIGN KEY (FkPosicaoIndicacao)
	REFERENCES PosicaoIndicacao on Update CASCADE ON DELETE CASCADE
   
);

Create TABLE Lembrete(

	idLembrete int IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Descricao nvarchar(250),
    DataCadastro datetime,
    DataLembrar datetime,
    FkUsuario int NOT NULL,
    
    CONSTRAINT UsuarioFK FOREIGN KEY (FkUsuario)
	REFERENCES Prospect(idProspect) on Update CASCADE ON DELETE CASCADE
    

);


drop table Usuario;