﻿using Elrond.TradeOffer.Web.BotWorkflows.Bids;
using Elrond.TradeOffer.Web.Database;
using Elrond.TradeOffer.Web.Models;

namespace Elrond.TradeOffer.Web.BotWorkflows.Offers;

public class Offer
{
    public Offer(
        Guid id,
        DateTime createdAt,
        ElrondNetwork network,
        long creatorUserId,
        long creatorChatId,
        TokenAmount amount,
        string description)
    {
        Id = id;
        Network = network;
        CreatorUserId = creatorUserId;
        CreatorChatId = creatorChatId;
        CreatedAt = createdAt;
        Amount = amount;
        Description = description;
    }

    public Guid Id { get; }
    
    public DateTime CreatedAt { get; }
    
    public ElrondNetwork Network { get; }
    
    public long CreatorUserId { get; }
    
    public long CreatorChatId { get; }
    
    public TokenAmount Amount { get; }

    public string Description { get; }

    public static Offer From(DbOffer dbOffer)
    {
        return new Offer(
            dbOffer.Id,
            dbOffer.CreatedAt,
            dbOffer.Network,
            dbOffer.CreatorUserId,
            dbOffer.CreatorChatId,
            TokenAmount.From(dbOffer.TokenAmount,
                new Token(dbOffer.TokenName, dbOffer.TokenIdentifier, dbOffer.TokenNonce, dbOffer.TokenPrecision)),
            dbOffer.Description);
    }
}