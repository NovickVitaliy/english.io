from flask import Flask, Response, request
from gtts import gTTS
from io import BytesIO
import redis
import logging
import hashlib

app = Flask(__name__)

redis_client = redis.StrictRedis(
    host="redis-cache",
    port=6379,
    db=0,
    decode_responses=False
)

logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(levelname)s - %(message)s",
    handlers=[
        logging.StreamHandler(),
    ]
)
logger = logging.getLogger(__name__)

def generate_audio_stream(text, lang="en"):
    audio_buffer = BytesIO()
    
    tts = gTTS(text=text, lang=lang)
    tts.write_to_fp(audio_buffer)
    
    audio_buffer.seek(0)
    return audio_buffer.getvalue()

def stream_from_bytes(audio_bytes):
    buffer = BytesIO(audio_bytes)
    while chunk := buffer.read(1024):
        yield chunk

@app.route("/api/tts", methods=["GET"])
def text_to_speech():
    text = request.args.get("text")
    if not text:
        return Response("Missing 'text' parameter", status=400)

    lang = request.args.get("lang", "en")

    cache_key = f"tts:{hashlib.md5(f'{text}:{lang}'.encode()).hexdigest()}"

    try:
        cached_audio = redis_client.get(cache_key)
        if cached_audio:
            logger.info(f"Audio retrieved from cache for text='{text}', lang='{lang}'")
            return Response(
                stream_from_bytes(cached_audio),
                mimetype="audio/mp3",
                headers={
                    "Content-Disposition": "inline; filename=tts.mp3",
                    "Accept-Ranges": "bytes"
                }
            )

        audio_bytes = generate_audio_stream(text, lang)
        logger.info(f"Audio generated and cached for text='{text}', lang='{lang}'")

        redis_client.setex(cache_key, 3600, audio_bytes)

        return Response(
            stream_from_bytes(audio_bytes),
            mimetype="audio/mp3",
            headers={
                "Content-Disposition": "inline; filename=tts.mp3",
                "Accept-Ranges": "bytes"
            }
        )
    except redis.RedisError as re:
        logger.error(f"Redis error: {str(re)}")
        return Response(f"Cache error: {str(re)}", status=500)
    except Exception as e:
        logger.error(f"Error generating audio: {str(e)}")
        return Response(f"Error generating audio: {str(e)}", status=500)

if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0", port=5003)