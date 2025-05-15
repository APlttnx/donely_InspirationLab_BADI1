CREATE TABLE Users (
    userID INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255),
    email VARCHAR(255) UNIQUE,
    telephone_nr VARCHAR(50),
    is_admin BOOLEAN,
    profile_picture VARCHAR(255),
    created DATETIME DEFAULT CURRENT_TIMESTAMP,
    last_login DATETIME DEFAULT CURRENT_TIMESTAMP,
);

CREATE TABLE User_Passwords (
    userID INT PRIMARY KEY,
    password VARCHAR(255),
    has_mfa BOOLEAN
    FOREIGN KEY (userID) REFERENCES Users(userID)
);

CREATE TABLE GroupsU (
    groupID INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255),
    owner INT,
    creation_date DATETIME,
    shop_active BOOLEAN,
    image VARCHAR(255),
    FOREIGN KEY (owner) REFERENCES Users(userID)
);

CREATE TABLE Group_Users (
    group_userID INT PRIMARY KEY AUTO_INCREMENT,
    userID INT,
    groupID INT,
    currency INT,
    joined DATETIME DEFAULT CURRENT_TIMESTAMP,
    role VARCHAR(50), -- expects 'owner' or 'member'
    FOREIGN KEY (userID) REFERENCES Users(userID),
    FOREIGN KEY (groupID) REFERENCES GroupsU(groupID)
);

CREATE TABLE Shop_Items (
    itemID INT PRIMARY KEY AUTO_INCREMENT,
    groupID INT,
    reward VARCHAR(255),
    cost INT,
    is_active BOOLEAN,
    FOREIGN KEY (groupID) REFERENCES GroupsU(groupID)
);

CREATE TABLE Bought_Items (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    itemID INT,
    groupUserID INT,
    time DATETIME,
    FOREIGN KEY (itemID) REFERENCES Shop_Items(itemID),
    FOREIGN KEY (groupUserID) REFERENCES Group_Users(group_userID)
);

CREATE TABLE Tasks_Definition (
    taskID INT PRIMARY KEY AUTO_INCREMENT,
    groupID INT,
    name VARCHAR(255),
    details VARCHAR(255),
    reward_currency INT,
    reward_custom VARCHAR(255),
    frequency VARCHAR(50), -- expects 4 options
    is_active BOOLEAN,
    created_on DATETIME,
    validation_required BOOLEAN,
    icon VARCHAR(255),
    FOREIGN KEY (groupID) REFERENCES GroupsU(groupID)
);

CREATE TABLE Task_Instances (
    taskInstanceID INT PRIMARY KEY AUTO_INCREMENT,
    taskID INT,
    groupUserID INT,
    status VARCHAR(50), -- expects in_progress/success/fail/pending_validation
    deadline DATETIME,
    issued_on DATETIME,
    completed_on DATETIME,
    FOREIGN KEY (taskID) REFERENCES Tasks_Definition(taskID),
    FOREIGN KEY (groupUserID) REFERENCES Group_Users(group_userID)
);
