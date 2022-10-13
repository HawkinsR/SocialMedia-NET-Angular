/*******************************************************************************
   Social Media Database - Version 1.0
   Script: SocialMediaSQL.sql
   Description: Creates and populates the Social Media database.
********************************************************************************/

/*******************************************************************************
   Drop Constraints
********************************************************************************/

    ALTER TABLE [smd].[Users]
        DROP CONSTRAINT [FK_User_Post];
    GO

/*******************************************************************************
   Drop Tables
********************************************************************************/

    DROP TABLE [smd].[Posts];
    GO

    DROP TABLE [smd].[Users];
    GO

/*******************************************************************************
   Drop Schema
********************************************************************************/

    DROP SCHEMA [smd];
    GO

/*******************************************************************************
   Create Schema
********************************************************************************/

    CREATE SCHEMA [smd];
    GO

/*******************************************************************************
   Create Tables
********************************************************************************/

    CREATE TABLE [smd].[Users]
    (
        [UserId] INT IDENTITY(1,1) NOT NULL,
        [UserFirstName] NVARCHAR(255) NOT NULL,
        [UserLastName] NVARCHAR(255) NOT NULL,
        [UserEmail] NVARCHAR(255) UNIQUE NOT NULL,
        [UserPassword] NVARCHAR(255) UNIQUE NOT NULL,
    );
    GO

    CREATE TABLE [smd].[Posts]
    (
        [PostId] INT IDENTITY(1,1) NOT NULL,
        [PostText] NVARCHAR(255) NOT NULL,
        [PostImageUrl] NVARCHAR(255),
        [PostAuthorId] INT NOT NULL,
        [PostReference] INT
    );
    GO

/*******************************************************************************
   Create Primary Key Constraints
********************************************************************************/

    ALTER TABLE [smd].[Users]
        ADD CONSTRAINT [PK_UserId]
        PRIMARY KEY ([UserId]);
    GO

    ALTER TABLE [smd].[Posts]
        ADD CONSTRAINT [PK_PostId]
        PRIMARY KEY ([PostId]);
    GO

/*******************************************************************************
   Create Foreign Key References
********************************************************************************/

    ALTER TABLE [smd].[Users]
        ADD CONSTRAINT [FK_User_Post]
        FOREIGN KEY ([UserId]) REFERENCES [smd].[Posts]([PostAuthorId]);
    GO

    ALTER TABLE [smd].[Posts]
        ADD CONSTRAINT [PK_PostId]
        FOREIGN KEY ([PostId]) REFERENCES [smd].[Posts]([PostId]);
    GO

/*******************************************************************************
   Seed Database
********************************************************************************/

    INSERT INTO [smd].[Users] ([UserFirstName], [UserLastName], [UserEmail], [UserPassword])
        VALUES
            ('Peter', 'Parker', 'PParker@columbia.edu', 'AuntM@y46'),
            ('Tony', 'Stark', 'TStark@starkindustries.com', 'Pep%er07'),
            ('Thor', 'Odinson', 'TOdinson@valhalla.net', 'Mjoln!r1'),
            ('Steve', 'Rogers', 'SRogers@army.mil', 'GetSw0le!');
    GO

    INSERT INTO [smd].[Posts] ([PostText], [PostImageUrl], [PostAuthorId], [PostReference])
        VALUES
            ('Ironman? More like Iron-Pan!', null, 1, null),
            ('Just finished Hydra once and for all! Now, off to date night!', null, 4, null),
            ('What do you mean dad''s not dead?! LOKI!!!', null, 3, null),
            ('Just when I thought I was out, these suits pull me back in! But seriously, anyone got a cutting torch?', null, 2, null),
            ('Watch it there Spidey, I know where you live. Oh, is your aunt still single?', null, 2, 1),
            ('That''s just wrong Stark!', null, 3, 5),
            ('Hahaha! Good one Spidey! You got him with a zinger there!', null, 4, 1),
            ('Nice!', null, 3, 2),
            ('Oh man, family sure is silly! I wish I had some...', null, 1, 3),
            ('Wow, from burn to self-burn. THAT takes some flexiblity! Well done web-slinger!', null, 2, 9),
            ('Oh no! Give me ten minutes to get uptown, I''ll save you Mr. Stark!', null, 1, 4),
            ('Hey! I saw that one! It was on my required viewing list! I didn''t get the thing with the horse though.', null, 4, 4),
            ('If you just grew some muscles, you could just BURST out of those puny suits wheneer you like! Nothing can withstand the strength of a god! Ask me about my fitness routine, I dare you!', null, 3, 4);
    GO
