/* Move to EF Migration
*/

SET IDENTITY_INSERT Units ON;
INSERT INTO Units( UnitId, ShortName, Description) VALUES 
	(1, 'шт','штуки'),
	(2, 'кг', 'килограммы'),
	(3, 'л', 'литры'),
	(4, 'м2', 'квадратные метры');
SET IDENTITY_INSERT Units OFF;


INSERT INTO Products( Name, Description, UnitId, Quantity, ImageSource) VALUES 
	('Шуруповерт Bosch', 'Дрель-шуруповерт Bosch GSB 12V', 1, 5, 'Screw1.jpg'),
	('Краска масляная', 'Краска масляная МА-15 зеленая', 3, 100, 'Color1.jpg'),
	('Керамогранит', 'Керамогранит ColiseumGres Портофино Бьянко белый 450х450х8 мм', 4, 30, 'Title1.jpg');
