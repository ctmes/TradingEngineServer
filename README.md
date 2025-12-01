# TradingEngineServer

A simple trading engine server written in C#.

## Overview

This trading engine implements a limit order book system for matching buy and sell orders. It provides infrastructure for order management, order book maintenance, and price discovery through a spread calculation mechanism.

## Architecture

The solution is organized into the following projects:

### TradingEngineServer (Core)
The main entry point and host for the trading engine. It uses Microsoft's Generic Host infrastructure to run as a background service.

- **Program.cs**: Entry point that builds and runs the trading engine host
- **TradingEngineServer.cs**: Background service that implements the main engine loop
- **TradingEngineServerHostBuilder.cs**: Configures dependency injection and services
- **iTradingEngine.cs**: Interface defining the trading engine contract

### OrdersCS (Orders Library)
Defines the order domain model and order-related operations:

- **Order**: Represents a limit order with price, quantity, and side (buy/sell)
- **OrderCore**: Core order properties (OrderID, Username, SecurityID)
- **IOrderCore**: Interface for order identity
- **ModifyOrder**: Represents an order modification request
- **CancelOrder**: Represents an order cancellation request
- **Limit**: A price level in the order book containing a linked list of orders
- **OrderbookEntry**: A node in the linked list at a specific price level
- **Side**: Enum for order side (Bid/Ask)
- **LimitComparer**: Custom comparers for sorting bid/ask limits

### OrderbookCS (Order Book Library)
Implements the central limit order book:

- **Orderbook**: The main order book implementation with:
  - Separate sorted sets for bid and ask limits
  - O(1) order lookup by ID using a dictionary
  - Linked list structure at each price level for order priority (FIFO)
- **OrderbookSpread**: Represents the best bid/ask spread
- **MatchResult**: Result of order matching (placeholder for matching engine)

**Interfaces:**
- **IReadOnlyOrderbook**: Read-only order book queries
- **IOrderEntryOrderbook**: Order entry operations (add, modify, remove)
- **IRetrievalOrderbook**: Order retrieval operations
- **IMatchingOrderbook**: Order matching interface

### InstrumentCS (Instruments Library)
Defines tradeable instruments:

- **Security**: Represents a tradeable security/instrument

## How the Order Book Works

### Data Structures

The order book uses three key data structures:

1. `SortedSet<Limit>` for bids: Sorted in descending price order (highest bid first)
2. `SortedSet<Limit>` for asks: Sorted in ascending price order (lowest ask first)
3. `Dictionary<long, OrderbookEntry>`: Maps order IDs to order book entries for O(1) lookup

### Order Flow

1. **Adding an Order**: 
   - A new limit price level is created if it doesn't exist
   - The order is appended to the tail of the linked list at that price level
   - The order is indexed in the dictionary by its ID

2. **Modifying an Order**:
   - The existing order is cancelled
   - A new order with modified parameters is added

3. **Cancelling an Order**:
   - The order is removed from its linked list
   - Head/tail pointers are updated appropriately
   - The order is removed from the dictionary

### Price Priority

- **Bids**: Higher prices have priority (buyers want to pay more)
- **Asks**: Lower prices have priority (sellers want to receive less)

### Time Priority

Orders at the same price level are processed in FIFO order using a doubly-linked list.

## Configuration

The engine is configured via `appsettings.json`:

```json
{
    "LoggerConfiguration": {
        "LoggerType": "Text",
        "TextLoggerConfiguration": {
            "Directory": "path/to/logs",
            "Filename": "TradingEngineServer",
            "FileExtension": ".log"
        }
    },
    "TradingEngineServerConfiguration": {
        "TradingEngineServerSettings": {
            "Port": 12000
        }
    }
}
```

## Dependencies

- Microsoft.Extensions.Hosting
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Logging
- External LoggingCS library (not included in this repository)

## License

See [LICENSE.txt](LICENSE.txt) for license information.
