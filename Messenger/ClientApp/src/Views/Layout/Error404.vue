<template>
    <div class="container">
        <div class="card border-danger mb-3 rounded">
            <div class="card-body">
                <div>404, Requested page not found.</div>
                <div>
                    
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import Button from "@/Views/Components/Button";
    export default {
        name: "Error404",
        components: {Button}
    }
</script>

<style scoped>
    .card-body {
        text-align: center;
    }
    .container {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        height: 100%;
    }
</style>