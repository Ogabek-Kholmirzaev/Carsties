async function getDataAsync() {
    const response = await fetch('http://localhost:6001/search', {cache: 'force-cache'});
    if (!response.ok) {
        throw new Error('Failed to fetch data');
    }

    return response.json();
}

export default async function Listings() {
    const data = await getDataAsync();

    return (
        <div>
            {JSON.stringify(data, null, 2)}
        </div>
    )
}