USE CRMLightDb;
GO

-- Les utilisateurs peuvent être initialisés via l'application.
-- Exemple de données pour tests :

INSERT INTO Clients(CodeClient, Nom, Entreprise, Email, Telephone, Adresse, Source, Statut)
VALUES
('CLI-0001', 'Alice Martin', 'AM Consulting', 'alice@example.com', '+237600000001', 'Douala', 'Web', 'Prospect'),
('CLI-0002', 'Benoit Kamga', 'Kamga Agency', 'benoit@example.com', '+237600000002', 'Yaoundé', 'Référencement', 'Client');
GO
