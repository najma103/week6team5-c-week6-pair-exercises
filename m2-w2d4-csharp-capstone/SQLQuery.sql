select * from park;

select camp.campground_id, camp.name, camp.open_from_mm, camp.open_to_mm, camp.daily_fee from park
join campground camp on camp.park_id = park.park_id
where park.park_id = 2

select * from campground where park_id = 2;

select site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, c.daily_fee from site 
join campground c on c.campground_id = site.campground_id
join park p on p.park_id = c.park_id
join reservation r on r.site_id = site.site_id
where r.from_date not between('2017-02-20') AND ('2017-02-24') 
	and r.to_date not between('2017-02-20') AND ('2017-02-24');


select * from site
join campground camp on camp.campground_id = site.campground_id
where camp.campground_id = 2;

select distinct  site.site_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, camp.daily_fee,
r.site_id, r.from_date, r.to_date, camp.campground_id from site
join campground camp on camp.campground_id = site.campground_id
left outer join reservation r on r.site_id = site.site_id
where camp.campground_id = 2 AND (r.from_date not between('2017-02-20') AND ('2017-02-24') 
			AND r.to_date not between('2017-02-20') AND ('2017-02-24'))


-- select sites for particular campground
select site.site_id, site.site_number, site.max_occupancy, site.max_rv_length, site.utilities from site
join campground camp on camp.campground_id = site.campground_id
where site.campground_id = 2;


select * from reservation;

insert into park values ('BallPark','Ohio','1879-07-07', 57898, 9999, 'Beautiful park you can balls, whatever balls you like');