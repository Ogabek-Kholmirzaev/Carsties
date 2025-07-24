import { Button } from "flowbite-react";
import Link from "next/link";

export default function UserActions() {
    return (
        <Button>
            <Link href='/session'>Session</Link>
        </Button>
    )
}