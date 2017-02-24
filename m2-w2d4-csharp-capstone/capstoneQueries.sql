SELECT * FROM park p
SELECT * FROM campground c
SELECT * FROM site s
SELECT * FROM reservation r


SELECT distinct r.site_id,r.from_date,r.to_date FROM reservation r
join site s ON r.site_id = s.site_id
WHERE r.from_date NOT BETWEEN('2017-02-25') AND ('2017-03-01')
AND r.to_date NOT BETWEEN ('2017-02-25') AND DATEADD(M,1,'2017-03-01');




--Need a query that selects all of the available Sites for a selected date range.
--This query needs to exclude any overlapping reservations and their associated site_ids.


--FOR all site_id select only those that do NOT have reservations in that overlap the
--requested stay date range. BELOW!
SELECT * from site s
WHERE s.campground_id = 3 AND s.site_id NOT IN
	(SELECT site_id FROM reservation r
	WHERE (r.to_date > '2017-02-25') AND (r.from_date < '2017-03-01'))








--separate
SELECT * FROM park p
join campground c ON p.park_id = c.park_id
join site s ON c.campground_id = s.campground_id
join reservation r ON s.site_id = r.site_id
WHERE datediff(day, getdate(),r.from_date) <= 2 AND datediff(day, getdate(),r.from_date) >= 0
ORDER BY r.from_date DESC;
--30 day reservation query below
SELECT r.name, r.from_date, r.to_date, r.reservation_id FROM park p
join campground c ON p.park_id = c.park_id
join site s ON c.campground_id = s.campground_id
join reservation r ON s.site_id = r.site_id
WHERE datediff(day, getdate(),r.from_date) <= 30 AND datediff(day, getdate(),r.from_date) >= 0
ORDER BY r.from_date;

--Query the Reservation_Id from a Provided Family Name, From & To Dates
SELECT reservation_id FROM reservation r 
WHERE r.name = 'Scott Family' AND r.from_date = '2017-03-29' AND r.to_date = '2017-04-03';


--Query to Select the Reservation_Id from the Most Recent Reservation
SELECT TOP 1(reservation_id) FROM reservation r
ORDER BY r.reservation_id DESC
