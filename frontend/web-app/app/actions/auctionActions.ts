'use server';

import { Auction, PagedResult } from "@/types";

export async function getDataAsync(pageNumber: number): Promise<PagedResult<Auction>> {
    const response = await fetch(`http://localhost:6001/search?pageSize=4&pageNumber=${pageNumber}`);
    if (!response.ok) {
        throw new Error('Failed to fetch data');
    }

    return response.json();
}