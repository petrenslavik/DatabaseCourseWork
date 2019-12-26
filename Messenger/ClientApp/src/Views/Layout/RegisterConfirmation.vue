<template>
    <div class="container">
        <div class="card border-danger mb-3 rounded">
            <div class="card-body">
                <div>In order to complete registration, you should verify your email.<br>
                    We send an message to provided email.</div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "RegisterConfirmation"
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