insert into blocks (text, sent_on, block_id, user_id)
values (@Text, @SentOn, @BlockId, @UserId)
returning id;