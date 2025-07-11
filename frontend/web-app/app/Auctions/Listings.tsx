import AuctionCard from "./AuctionCard";

async function getDataAsync() {
    const response = await fetch('http://localhost:6001/search?pageSize=10');
    if (!response.ok) {
        throw new Error('Failed to fetch data');
    }

    return response.json();
}

export default async function Listings() {
    const data = await getDataAsync();

    return (
        <div className="grid grid-cols-4 gap-6">
            {data && data.results.map((auction: any) => (
                <AuctionCard key={auction.id} auction={auction} />
            ))}
        </div>
    )
}