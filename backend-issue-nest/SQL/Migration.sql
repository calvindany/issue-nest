-- Step 1 Create Database
CREATE DATABASE issue_nest;


-- Step 2 Create Required Table
CREATE TABLE ms_users (
	pk_ms_user INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	name VARCHAR(100) NOT NULL,
	email VARCHAR(60) NOT NULL,
	role VARCHAR(20) CHECK (role IN ('Admin', 'User')) NOT NULL,
	password TEXT NOT NULL,
	
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE tr_tickets (
    pk_tr_tickets INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    title VARCHAR(100) NOT NULL,
    description TEXT NOT NULL,
    status NVARCHAR(20) NOT NULL CHECK (status IN ('Open', 'In Progress', 'Resolved')),
    client_id INT NOT NULL,
    admin_response TEXT NULL,

    created_at DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updated_at DATETIME DEFAULT NULL
);


-- Step 3 Seed Dummy Data for ms_users and tr_tickets
INSERT INTO ms_users (name, email, role, password, created_at)
VALUES 
('Alice Johnson', 'alice@example.com', 'User', 'password123', DEFAULT),
('Bob Smith', 'bob@example.com', 'User', 'password123', DEFAULT),
('Charlie Brown', 'bob@example.com', 'User', 'password123', DEFAULT),
('Dana White', 'dana@example.com', 'User', 'password123', DEFAULT),
('Eva Green', 'eva@example.com', 'User', 'password123', DEFAULT),
('Frank Knight', 'frank@example.com', 'Admin', 'password123', DEFAULT);

INSERT INTO tr_tickets (title, description, status, client_id, admin_response, created_at, updated_at)
VALUES 
('Issue with login', 'Users are unable to login to the system.', 'Open', 1, NULL, DEFAULT, DEFAULT),
('Payment gateway error', 'Error during payment processing.', 'In Progress', 2, NULL, DEFAULT, DEFAULT),
('Bug in dashboard', 'Dashboard shows incorrect data.', 'Resolved', 3, 'Issue resolved by fixing the data source.', DEFAULT, DEFAULT),
('Feature request', 'Request for adding a new feature.', 'Open', 4, NULL, DEFAULT, DEFAULT),
('UI not responsive', 'The user interface is not responsive on mobile.', 'Open', 5, NULL, DEFAULT, DEFAULT),
('Error 404 on page', 'Users encounter error 404 on certain pages.', 'In Progress', 1, NULL, DEFAULT, DEFAULT),
('Slow performance', 'The application is running slow during peak hours.', 'Open', 2, NULL, DEFAULT, DEFAULT),
('Data export issue', 'Unable to export data in CSV format.', 'Open', 3, NULL, DEFAULT, DEFAULT),
('Login attempts not recorded', 'Failed login attempts are not being logged.', 'Resolved', 4, 'The logging issue has been fixed.', DEFAULT, DEFAULT),
('Profile update issue', 'Users cannot update their profile information.', 'In Progress', 5, NULL, DEFAULT, DEFAULT);
