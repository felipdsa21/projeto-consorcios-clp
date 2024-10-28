CREATE TABLE Usuarios (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(100) NOT NULL,
);

CREATE TABLE Consorcios (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(100) NOT NULL,
    "ValorTotal" DECIMAL NOT NULL,
    "DataInicio" DATE NOT NULL,
    "DataFim" DATE NOT NULL,
    "NumeroParticipantes" INT NOT NULL,
    "Status" VARCHAR(50) NOT NULL,
    "Parcelas" INT NOT NULL
);

CREATE TABLE Participa (
    "ConsorcioId" SERIAL REFERENCES "Consorcios" ("Id"),
    "UsuarioId" SERIAL REFERENCES "Usuarios" ("Id"),
    "DataEntrada" TIMESTAMP NOT NULL,
    "Status" VARCHAR(50) NOT NULL,
    "PRIMARY" KEY ("ConsorcioId", "UsuarioId")
);
