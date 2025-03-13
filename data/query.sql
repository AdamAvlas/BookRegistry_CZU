--CREATE TABLE categories
--(
--	category_id INTEGER PRIMARY KEY IDENTITY(0,1),
--	category_name nvarchar(50) NOT NULL
--)

--CREATE TABLE authors
--(
--	author_id INTEGER PRIMARY KEY IDENTITY(0,1),
--	first_name nvarchar(40) NOT NULL,
--	last_name nvarchar(40) NOT NULL,
--	birthdate date
--)

--CREATE TABLE books
--(
--	book_id INTEGER PRIMARY KEY IDENTITY(0,1),
--	title nvarchar(75) NOT NULL,
--	category_id INTEGER FOREIGN KEY REFERENCES categories,
--	author_id INTEGER FOREIGN KEY REFERENCES authors,
--	date_added date
--)

/*INSERT INTO categories(category_name) VALUES('Fantasy'),('Sci-fi'),('Naučné');
INSERT INTO authors(first_name,last_name,birthdate) VALUES ('J.R.R.','Tolkien','01-01-1905');*/

--INSERT INTO books(title,category_id,author_id,date_added) VALUES('Hobbit',0,0, GETDATE())

--SELECT title,date_added, category_name, first_name, last_name
--FROM books
--INNER JOIN categories ON books.category_id = categories.category_id
--INNER JOIN authors ON books.author_id = authors.author_id

SELECT TOP(100) author_id, first_name, last_name, birthdate FROM authors
SELECT TOP(100) category_id, category_name FROM categories
SELECT TOP(100) book_id, title, category_id, author_id FROM books