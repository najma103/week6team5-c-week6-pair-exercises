select camp.name, camp.open_from_mm, camp.open_to_mm, camp.daily_fee from park
join campground camp on camp.park_id = park.park_id
where park.park_id = 1


-- select sites for particular campground
select site.site_id, site.site_number, site.max_occupancy, site.max_rv_length, site.utilities from site
join campground camp on camp.campground_id = site.campground_id
where site.campground_id = 2;

select * from reservation;