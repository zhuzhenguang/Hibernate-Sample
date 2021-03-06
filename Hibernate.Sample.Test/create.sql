create table t_user(
  id bigint identity(1,1) primary key,
  first_name nvarchar(512),
  last_name nvarchar(512),
  telephone varchar(30),
  age int,
  group_id int
);
  
create table t_passport(
  id bigint primary key FOREIGN KEY REFERENCES t_user(id),
  serial varchar(30),
  expiry int
);
  
create table t_group(
  id bigint identity(1,1) primary key,
  name nvarchar(512)
);

create table t_address(
  id bigint identity(1,1) primary key,
  address nvarchar(512),
  zipcode nvarchar(512),
  user_id bigint FOREIGN KEY REFERENCES t_user(id)
);

alter table t_address alter column user_id bigint not null;

create table t_role(
  id bigint identity(1,1) primary key,
  name nvarchar(512)
);

create table t_group_role(
  group_id bigint FOREIGN KEY REFERENCES t_group(id),
  role_id bigint FOREIGN KEY REFERENCES t_role(id)
);

create table t_book1(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  manufacturer nvarchar(512),
  pagecount int
);

create table t_dvd1(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  manufacturer nvarchar(512),
  regincode varchar(30)
);

create table t_item2(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  manufacturer nvarchar(512)
);

create table t_book2(
  id bigint primary key FOREIGN KEY REFERENCES t_item2(id),
  pagecount int
);

create table t_dvd2(
  id bigint primary key FOREIGN KEY REFERENCES t_item2(id),
  regioncode varchar(30)
);

create table t_item3(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  manufacturer nvarchar(512),
  pagecount int,
  regioncode varchar(30),
  category int
);

alter table t_user add resume nvarchar(max);

create table t_user2(
  id bigint primary key,
  name nvarchar(512)
);

alter table t_address alter column user_id bigint null;

alter table t_user2 add version bigint;

alter table t_user2 add resume nvarchar(4000) not null;

alter table t_user2 alter column resume nvarchar(4000) null;

create table t_user3(
  id bigint identity(1,1) primary key,
  name nvarchar(512)
);

create table t_address2(
  id bigint identity(1,1) primary key,
  detail nvarchar(512),
  user_id bigint not null
);

create table t_address3(
  id bigint identity(1,1) primary key,
  detail nvarchar(512),
  type nvarchar(512),
  user_id bigint not null
);

create table t_address4(
  id bigint identity(1,1) primary key,
  detail nvarchar(512),
  idx bigint,
  user_id bigint not null
);