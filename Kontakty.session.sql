CREATE TABLE Kontakty (
        first_name TEXT NOT NULL,
        last_name  TEXT NOT NULL,
        email TEXT PRIMARY KEY, 
        pass PASSWORD, 
        category TEXT,
        phonenr DECIMAL(9,0),
        birthday DATE,
        FOREIGN KEY (category) REFERENCES Kategoria(category)

)