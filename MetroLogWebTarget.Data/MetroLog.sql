CREATE TABLE [Environment] (
  [Id] INT NOT NULL ON CONFLICT FAIL, 
  [PackageArchitecture] NVARCHAR(32), 
  [PackageFullName] NVARCHAR(128), 
  [PackagePublisher] VARCHAR(64), 
  [PackagePublisherId] NVARCHAR(32), 
  [PackageResourceId] NVARCHAR(128), 
  [PackageVersion] NVARCHAR(32), 
  [InstallationId] NVARCHAR(32), 
  CONSTRAINT [] PRIMARY KEY ([Id] ASC) ON CONFLICT FAIL);

CREATE UNIQUE INDEX [PubId] ON [Environment] ([PackagePublisherId] COLLATE NOCASE ASC);


CREATE TABLE [Event] (
  [Id] INT NOT NULL ON CONFLICT FAIL, 
  [BatId] INT NOT NULL, 
  [SequenceId] INT, 
  [Logger] NVARCHAR(128), 
  [Level] NVARCHAR(32), 
  [Message] NVARCHAR, 
  [Exception] NVARCHAR, 
  [TimeStamp] DATETIME);
