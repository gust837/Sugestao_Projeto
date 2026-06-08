CREATE DATABASE Sugestao_Senai
GO

USE Sugestao_Senai
GO

CREATE TABLE Usuario(
	Id INT IDENTITY PRIMARY KEY,
	Nome VARCHAR(100) NOT NULL,
	Cpf VARCHAR(50) NOT NULL,
	Email VARCHAR(100) NOT NULL UNIQUE,
	Adm BIT NOT NULL
);
GO

CREATE TABLE Sugestao(
	Id INT IDENTITY PRIMARY KEY,
	Nome VARCHAR(100) NOT NULL,
	Descricao VARCHAR(MAX) NOT NULL,
	StatusSugestao CHAR(1) NOT NULL
		CHECK (StatusSugestao IN ('A', 'E', 'F')), -- A = Andamento, E = Espera, F = Finalizado
	DataStatus DATE NOT NULL,
	DataSugestao DATE NOT NULL,
	Imagem VARCHAR(MAX),
	Localizacao CHAR(1) NOT NULL
		CHECK (Localizacao IN ('T', '1', '2', 'C', 'R')), -- T = Térreo, 1 = 1ºAndar, 2 = 2ºAndar, C = Coworking, R = Refeitorio

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
INSERT INTO Usuario (Nome, Cpf, Email, Adm) VALUES
('Ana Silva', '123.456.789-00', 'ana.silva@email.com', 1),    -- Administradora
('Bruno Costa', '234.567.890-11', 'bruno.costa@email.com', 0),  -- Usuário Comum
('Carlos Souza', '345.678.901-22', 'carlos.souza@email.com', 0); -- Usuário Comum
GO

-- 2. Populando a tabela Categoria
INSERT INTO Categoria (Nome) VALUES
('Infraestrutura'),
('Sustentabilidade'),
('Tecnologia');
GO

-- 3. Populando a tabela Sugestao (Relacionada com Usuario e com Nova Coluna Localizacao)
INSERT INTO Sugestao (Nome, Descricao, StatusSugestao, DataStatus, DataSugestao, Imagem, Localizacao, UsuarioId) VALUES
(
    'Melhoria no Wi-Fi', 
    'Instalar novos roteadores no refeitório.', 
    'A', 
    '2026-06-01', 
    '2026-06-01', 
    '', 
    'R', -- R = Refeitório (conforme a descrição)
    2
), 
(
    'Coleta Seletiva', 
    'Implantar lixeiras de reciclagem nas salas.', 
    'E', 
    '2026-06-02', 
    '2026-06-02', 
    '', 
    '1', -- 1 = 1º Andar
    3
), 
(
    'Ar-condicionado', 
    'Manutenção preventiva dos aparelhos do bloco B.', 
    'F', 
    '2026-06-05', 
    '2026-05-20', 
    '', 
    'T', -- T = Térreo
    2
);
GO

-- 4. Populando a tabela Comentario (Relacionada com Usuario e Sugestao)
INSERT INTO Comentario (Descricao, DataComentario, UsuarioId, SugestaoId) VALUES
('Excelente ideia, o sinal cai muito lá.', '2026-06-02', 3, 1), -- Carlos comentou na sugestão 1
('Já estamos cotando os novos aparelhos.', '2026-06-03', 1, 1),  -- Ana (Adm) comentou na sugestão 1
('Obrigado pela agilidade na manutenção!', '2026-06-06', 2, 3); -- Bruno comentou na sugestão 3
GO

-- 5. Populando a tabela Usuario_Voto (Relacionada com Usuario e Sugestao)
INSERT INTO Usuario_Voto (UsuarioId, SugestaoId) VALUES
(1, 1), -- Ana votou na sugestão do Wi-Fi
(3, 1), -- Carlos votou na sugestão do Wi-Fi
(2, 2); -- Bruno votou na sugestão da Coleta Seletiva
GO

-- 6. Populando a tabela Sugestao_Categoria (Relacionada com Sugestao e Categoria)
INSERT INTO Sugestao_Categoria (SugestaoId, CategoriaId) VALUES
(1, 3), -- 'Melhoria no Wi-Fi' associada a 'Tecnologia'
(2, 2), -- 'Coleta Seletiva' associada a 'Sustentabilidade'
(3, 1); -- 'Ar-condicionado' associada a 'Infraestrutura'
GO

SELECT * FROM Sugestao