﻿using Telegram.Bot;

namespace Elrond.TradeOffer.Web.BotWorkflows.Workflows;

public interface IOfferNavigation
{
    Task ShowOffersAsync(ITelegramBotClient client, long userId, long chatId, CancellationToken ct);

    Task ShowOfferAsync(ITelegramBotClient client, long userId, long chatId, Guid offerId, CancellationToken ct);
}