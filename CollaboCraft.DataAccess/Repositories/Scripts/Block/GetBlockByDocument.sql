select id        as Id,
       text      as Text,
       sent_on   as SentOn,
       block_id   as BlockId,
       user_id   as UserId,
       edited_on as EditedOn
from blocks
where block_id = @blockId
  and sent_on >= @from
order by sent_on;