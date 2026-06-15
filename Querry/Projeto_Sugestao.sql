CREATE DATABASE Sugestao_Senai
GO

USE Sugestao_Senai
GO

CREATE TABLE Usuario(
	Id INT IDENTITY PRIMARY KEY,
	Nome VARCHAR(100) NOT NULL,
	Cpf VARCHAR(11) NOT NULL,
	Email VARCHAR(100) NOT NULL UNIQUE,
	Senha VARCHAR(100) NOT NULL UNIQUE,
	Adm BIT NOT NULL
);
GO

CREATE TABLE Sugestao(
	Id INT IDENTITY PRIMARY KEY,
	Nome VARCHAR(100) NOT NULL,
	Descricao VARCHAR(MAX),
	StatusSugestao CHAR(1) NOT NULL
		CHECK (StatusSugestao IN ('A', 'E', 'F', 'R')), -- A = Andamento, E = Espera, F = Finalizado, R = Recusado
	DataStatus DATE NOT NULL,
	DataSugestao DATE NOT NULL,
	Imagem VARCHAR(MAX),
	Localizacao CHAR(1) NOT NULL
		CHECK (Localizacao IN ('T', '1', '2', 'C', 'R')), -- T = T�rreo, 1 = 1�Andar, 2 = 2�Andar, C = Coworking, R = Refeitorio
	Votos INT DEFAULT 0 NOT NULL,

	UsuarioId INT NOT NULL FOREIGN KEY(UsuarioId)
	REFERENCES Usuario(Id) 
);
GO

CREATE TABLE Categoria(
	Id INT IDENTITY PRIMARY KEY,
	Nome VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Comentario(
	Id INT IDENTITY PRIMARY KEY,
	Descricao VARCHAR(MAX) NOT NULL,
	DataComentario DATE NOT NULL,

	UsuarioId INT NOT NULL FOREIGN KEY(UsuarioId)
	REFERENCES Usuario(Id),

	SugestaoId INT NOT NULL FOREIGN KEY(SugestaoId)
	REFERENCES Sugestao(Id)
);
GO

CREATE TABLE Usuario_Voto(
	UsuarioId INT NOT NULL FOREIGN KEY(UsuarioId)
	REFERENCES Usuario(Id),

	SugestaoId INT NOT NULL FOREIGN KEY(SugestaoId)
	REFERENCES Sugestao(Id)
);
GO

CREATE TABLE Sugestao_Categoria(
	SugestaoId INT NOT NULL FOREIGN KEY(SugestaoId)
	REFERENCES Sugestao(Id),

	CategoriaId INT NOT NULL FOREIGN KEY(CategoriaId)
	REFERENCES Categoria(Id)
);
GO

-- 1. Populando a tabela Usuario
INSERT INTO Usuario (Nome, Cpf, Email, Senha, Adm) VALUES
('Ana Silva', '12345678900', 'ana.silva@email.com', '123456',1),    -- Administradora
('Bruno Costa', '23456789011', 'bruno.costa@email.com', '654321', 0),  -- Usu�rio Comum
('Carlos Souza', '34567890122', 'carlos.souza@email.com', 'qwerty', 0); -- Usu�rio Comum
GO

-- 2. Populando a tabela Categoria
INSERT INTO Categoria (Nome) VALUES
('Infraestrutura'),
('Sustentabilidade'),
('Tecnologia');
GO

-- 3. Populando a tabela Sugestao (Relacionada com Usuario e com Nova Coluna Localizacao)
INSERT INTO Sugestao (Nome, Descricao, StatusSugestao, DataStatus, DataSugestao, Imagem, Localizacao, Votos, UsuarioId) VALUES
(
    'Melhoria no Wi-Fi', 
    'Instalar novos roteadores no refeit�rio.', 
    'A', 
    '2026-06-01', 
    '2026-06-01', 
    '', 
    'R', -- R = Refeit�rio (conforme a descri��o)
    1,
	2
), 
(
    'Coleta Seletiva', 
    'Implantar lixeiras de reciclagem nas salas.', 
    'E', 
    '2026-06-02', 
    '2026-06-02', 
    '', 
    '1', -- 1 = 1� Andar
    0,
	3
), 
(
    'Ar-condicionado', 
    'Manuten��o preventiva dos aparelhos do bloco B.', 
    'F', 
    '2026-06-05', 
    '2026-05-20', 
    '', 
    'T', -- T = T�rreo
    0,
	2
);
GO

-- 4. Populando a tabela Comentario (Relacionada com Usuario e Sugestao)
INSERT INTO Comentario (Descricao, DataComentario, UsuarioId, SugestaoId) VALUES
('Excelente ideia, o sinal cai muito l�.', '2026-06-02', 3, 1), -- Carlos comentou na sugest�o 1
('J� estamos cotando os novos aparelhos.', '2026-06-03', 1, 1),  -- Ana (Adm) comentou na sugest�o 1
('Obrigado pela agilidade na manuten��o!', '2026-06-06', 2, 3); -- Bruno comentou na sugest�o 3
GO

-- 5. Populando a tabela Usuario_Voto (Relacionada com Usuario e Sugestao)
INSERT INTO Usuario_Voto (UsuarioId, SugestaoId) VALUES
(1, 1), -- Ana votou na sugest�o do Wi-Fi
(3, 1), -- Carlos votou na sugest�o do Wi-Fi
(2, 2); -- Bruno votou na sugest�o da Coleta Seletiva
GO

-- 6. Populando a tabela Sugestao_Categoria (Relacionada com Sugestao e Categoria)
INSERT INTO Sugestao_Categoria (SugestaoId, CategoriaId) VALUES
(1, 3), -- 'Melhoria no Wi-Fi' associada a 'Tecnologia'
(2, 2), -- 'Coleta Seletiva' associada a 'Sustentabilidade'
(3, 1); -- 'Ar-condicionado' associada a 'Infraestrutura'
GO