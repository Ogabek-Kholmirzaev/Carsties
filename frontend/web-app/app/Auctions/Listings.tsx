import { Auction, PagedResult } from "@/types";
import AuctionCard from "./AuctionCard";
import AppPagination from "../components/AppPagination";

async function getDataAsync(): Promise<PagedResult<Auction>> {
    const response = await fetch('http://localhost:6001/search');
    if (!response.ok) {
        throw new Error('Failed to fetch data');
    }

    return response.json();
}

export default async function Listings() {
    const data = await getDataAsync();

    return (
        <>
            <div className="grid grid-cols-4 gap-6">
                {data && data.results.map((auction => (
                    <AuctionCard key={auction.id} auction={auction} />
                )))}
            </div>
            <div className="flex justify-center mt-4">
                <AppPagination currentPage={1} pageCount={data.pageCount} />
            </div>
        </>
    )
}