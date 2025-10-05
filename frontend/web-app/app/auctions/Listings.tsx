'use client';

import AuctionCard from "./AuctionCard";
import AppPagination from "../components/AppPagination";
import { useEffect, useState } from "react";
import { getDataAsync } from "../actions/auctionActions";
import Filters from "./Filters";
import { useParamsStore } from "@/hooks/useParamsStore";
import { useShallow } from "zustand/shallow";
import queryString from "query-string";
import EmptyFilter from "../components/EmptyFilter";
import { useAuctionStore } from "@/hooks/useAuctionStore";

export default function Listings() {
    const [loading, setLoading] = useState(true);

    const params = useParamsStore(useShallow(state => ({
        pageNumber: state.pageNumber,
        pageSize: state.pageSize,
        searchTerm: state.searchTerm,
        orderBy: state.orderBy,
        filterBy: state.filterBy,
        seller: state.seller,
        winner: state.winner
    })));
    const setParams = useParamsStore(state => state.setParams);

    const data = useAuctionStore(useShallow(state => ({
        auctions: state.auctions,
        totalCount: state.totalCount,
        pageCount: state.pageCount
    })));
    const setData = useAuctionStore(state => state.setData);

    const url = queryString.stringifyUrl({ url: '', query: params }, { skipEmptyString: true });

    function setPageNumber(pageNumber: number) {
        setParams({ pageNumber: pageNumber });
    }

    useEffect(() => {
        getDataAsync(url).then(data => {
            setData(data);
            setLoading(false);
        })
    }, [url, setData]);

    if (loading) {
        return <h3>Loading...</h3>
    }

    return (
        <>
            <Filters />
            {data.totalCount === 0
                ? (
                    <EmptyFilter showReset />
                )
                : (
                    <>
                        <div className="grid grid-cols-4 gap-6">
                            {data && data.auctions.map(auction => (
                                <AuctionCard key={auction.id} auction={auction} />
                            ))}
                        </div>
                        <div className="flex justify-center mt-4">
                            <AppPagination
                                pageChanged={setPageNumber}
                                currentPage={params.pageNumber}
                                pageCount={data.pageCount}
                            />
                        </div>
                    </>
                )
            }
        </>
    )
}